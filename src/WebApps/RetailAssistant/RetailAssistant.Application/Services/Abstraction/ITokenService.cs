using IdentityModel.Client;

namespace RetailAssistant.Application.Services.Abstraction;

public interface ITokenService
{
    public Task<TokenResponse> GetToken(string scope);
}
