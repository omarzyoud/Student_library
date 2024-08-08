using Microsoft.AspNetCore.Identity;

namespace SCR.API.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
