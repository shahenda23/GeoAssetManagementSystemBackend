using Microsoft.AspNetCore.Identity;

namespace GeoAssetManagementSystem.Models
{
    public class User : IdentityUser
    {
        //public int Id { get; set; }
        //public string Username { get; set; }
        //public string UserEmail { get; set; }
        //public string Password { get; set; }

        public List<Location>? Locations { get; set; } = new();
    }
}
