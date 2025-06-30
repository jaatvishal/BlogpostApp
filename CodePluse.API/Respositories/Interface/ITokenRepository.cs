using Microsoft.AspNetCore.Identity;

namespace CodePluse.API.Respositories.Interface
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
