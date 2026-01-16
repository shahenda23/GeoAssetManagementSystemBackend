using Microsoft.AspNetCore.Identity;

namespace GeoAssetManagementSystem.Models
{
    public class User : IdentityUser
    {
        public List<Location>? Locations { get; set; } = new();
    }
}
