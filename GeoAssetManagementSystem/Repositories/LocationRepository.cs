using GeoAssetManagementSystem.Models; 
using GeoAssetManagementSystem.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GeoAssetManagementSystem.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly LocationContext _context;

        public LocationRepository(LocationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Location>> GetAllAsync(string userId)
        {
            return await _context.Locations
                .Where(l => l.UserId == userId)
                .ToListAsync();
        }

        public async Task<Location?> GetByIdAsync(int id, string userId)
        {
            return await _context.Locations
                .FirstOrDefaultAsync(l => l.ID == id && l.UserId == userId);
        }

        public async Task AddAsync(Location location)
        {
            await _context.Locations.AddAsync(location);
        }

        public async Task UpdateAsync(Location location)
        {
            _context.Locations.Update(location);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Location location)
        {
            _context.Locations.Remove(location);
            await Task.CompletedTask;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<IEnumerable<Location>> SearchByNameAsync(string name, string userId)
        {
            return await _context.Locations
                .Where(l => l.UserId == userId && l.Name.Contains(name))
                .ToListAsync();
        }
    }
}