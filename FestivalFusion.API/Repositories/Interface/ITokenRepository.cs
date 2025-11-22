using Microsoft.AspNetCore.Identity;

namespace FestivalFusion.API.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
