using ShopApp.Entities;

namespace ShopApp.Services.Interfaces
{
    public interface ITokenService
    {
        string GetToken(AppUser user, IList<string> roles);
    }
}
