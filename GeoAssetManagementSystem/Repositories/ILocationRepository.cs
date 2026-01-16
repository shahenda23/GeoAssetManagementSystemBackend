using GeoAssetManagementSystem.Models;

namespace GeoAssetManagementSystem.Interfaces
{
    public interface ILocationRepository
    {
        Task<IEnumerable<Location>> GetAllAsync(string userId);
        Task<Location?> GetByIdAsync(int id, string userId);
        Task AddAsync(Location location);
        Task UpdateAsync(Location location);
        Task DeleteAsync(Location location);
        Task<bool> SaveChangesAsync();
        Task<IEnumerable<Location>> SearchByNameAsync(string name, string userId);
    }
}