using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GeoAssetManagementSystem.Models
{
    public class LocationContext : IdentityDbContext<User>
    {
        public DbSet<Location> Locations { get; set; }

        public LocationContext(DbContextOptions Options) : base(Options)
        {

        }

    }
}
