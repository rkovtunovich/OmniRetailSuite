using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace Infrastructure.SecretManagement.Vault;

public class VaultSecretManager : ISecretManager, IDisposable
{
    private readonly IVaultClient _vaultClient;
    private readonly Dictionary<string, IGetSecretCommand> _commands = [];
    private readonly ConcurrentDictionary<string, Task> _renewableTasks = [];
    private readonly CancellationTokenSource _cts = new();
    private readonly ILogger<VaultSecretManager> _logger;

    public VaultSecretManager(IVaultClient vaultClient, ILogger<VaultSecretManager> logger)
    {
        _vaultClient = vaultClient;

        _commands.Add("kv", new GetKeyValueSecretCommand(vaultClient));
        _commands.Add("database", new GetDatabaseSecretCommand(vaultClient));
        _logger = logger;
    }

    public async Task<Dictionary<string, string>> GetSecretAsync(SecretRequest request)
    {
        if (!_commands.TryGetValue(request.Namespace, out var command))
            throw new InvalidOperationException($"No command found for namespace '{request.Namespace}'.");

        var result = await command.ExecuteAsync(request);

        TryRunSecretRenewing(request, result);

        return result.Data;
    }

    public void TryRunSecretRenewing(SecretRequest request, VaultSecretResponse vaultSecretResponse)
    {
        var taskId = request.GetHashCode().ToString();

        if (_renewableTasks.ContainsKey(taskId))
            return;

        if (!vaultSecretResponse.Renewable || vaultSecretResponse.LeaseDuration <= 0)
            return;

        var renewTask = Task.Run(async () => 
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(vaultSecretResponse.LeaseDuration / 2), _cts.Token);
                    await _vaultClient.V1.System.RenewLeaseAsync(vaultSecretResponse.LeaseId, vaultSecretResponse.LeaseDuration);

                    _logger.LogInformation($"Lease renewed for {request.Path} with lease ID {vaultSecretResponse.LeaseId}");
                }
                catch (OperationCanceledException ex)
                {
                    _logger.LogInformation($"Renewable task for secret {request.Path} was cancelled. {ex.Message}");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error renewing secret {request.Path}");
                    break;
                }
                finally
                {
                    _renewableTasks.TryRemove(taskId, out _);

                    _logger.LogInformation($"Renewal task removed for secret {request.Path}, task: {taskId}");
                }
            }
        }, _cts.Token);

        _renewableTasks.TryAdd(taskId, renewTask);

        _logger.LogInformation($"Renewable task started for secret {request.Path}, task: {taskId}");
    }

    public void Dispose()
    {
        _logger.LogInformation("Disposing VaultSecretManager and cancelling tasks...");

        _cts.Cancel();

        foreach (var task in _renewableTasks.Values)
        {
            try
            {
                task.Wait();
            }
            catch (AggregateException ex)
            {
                _logger.LogError(ex, "Error waiting for task to complete");
            }
        }

        _cts.Dispose();
    }
}
