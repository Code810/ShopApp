using Microsoft.AspNetCore.Identity;

namespace ShopApp.Entities
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
