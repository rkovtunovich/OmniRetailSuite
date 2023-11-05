using IdentityModel.Client;

namespace BackOffice.Client.Services;

public interface ITokenService
{
    public Task<TokenResponse> GetToken(string scope);
}
