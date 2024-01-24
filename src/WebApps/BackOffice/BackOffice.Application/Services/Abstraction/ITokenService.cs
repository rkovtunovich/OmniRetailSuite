using IdentityModel.Client;

namespace BackOffice.Application.Services.Abstraction;

public interface ITokenService
{
    public Task<TokenResponse> GetToken(string scope);
}
