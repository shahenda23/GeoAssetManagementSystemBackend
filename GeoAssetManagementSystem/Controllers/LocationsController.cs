using System.Net.NetworkInformation;
using GeoAssetManagementSystem.DTOs;
using GeoAssetManagementSystem.Interfaces;
using GeoAssetManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GeoAssetManagementSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LocationsController : ControllerBase
    {
        
        private readonly ILocationRepository _repository;

        public LocationsController(ILocationRepository repository)
        {
            _repository = repository;
        }

        // Helper method to get the logged-in User's ID from the JWT token
        //دى اللى هتجيب اليوزر id اللى جوا التوكن
        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

        [HttpGet] //to get data
        public async Task<ActionResult<IEnumerable<LocationReadDto>>> GetLocations([FromQuery] string? name)
        {
            IEnumerable<Location> locations;

            if (!string.IsNullOrEmpty(name))
            {
                // Requirement: Search for locations by name
                locations = await _repository.SearchByNameAsync(name, UserId);
            }
            else
            {
                // Requirement: List all locations for the current user
                locations = await _repository.GetAllAsync(UserId);
            }

            var results = locations.Select(l => new LocationReadDto
            {
                Id = l.ID,
                Name = l.Name,
                Latitude = l.Latitude,
                Longitude = l.Longitude,
                Description = l.Description,
                DateTime = l.DateTime
            });

            return Ok(results);
        }

        [HttpPost] //to create location
        public async Task<ActionResult<LocationReadDto>> CreateLocation(LocationCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var location = new Location
            {
                Name = dto.Name,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Description = dto.Description,
                UserId = UserId 
            };

            await _repository.AddAsync(location);
            await _repository.SaveChangesAsync();

            var result = new LocationReadDto
            {
                Id = location.ID,
                Name = location.Name,
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                Description = location.Description,
                DateTime = location.DateTime
            };

            return CreatedAtAction(nameof(GetLocations), new { id = location.ID }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLocation(int id, LocationUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dto.Id) return BadRequest("ID mismatch");

            var existingLocation = await _repository.GetByIdAsync(id, UserId);
            if (existingLocation == null) return NotFound();

            existingLocation.Name = dto.Name;
            existingLocation.Latitude = dto.Latitude;
            existingLocation.Longitude = dto.Longitude;
            existingLocation.Description = dto.Description;

            await _repository.UpdateAsync(existingLocation);
            await _repository.SaveChangesAsync();

            return Ok(existingLocation);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            var location = await _repository.GetByIdAsync(id, UserId);
            if (location == null) return NotFound();

            await _repository.DeleteAsync(location);
            await _repository.SaveChangesAsync();

            return NoContent();
        }


    }
}
