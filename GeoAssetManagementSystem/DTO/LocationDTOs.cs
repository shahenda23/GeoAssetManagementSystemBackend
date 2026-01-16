using System.ComponentModel.DataAnnotations;

namespace GeoAssetManagementSystem.DTOs
{
    // For sending data to the Angular Map (GET)
    public class LocationReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string? Description { get; set; }
        public DateTime DateTime { get; set; }
    }

    // For creating a new location (POST)
    public class LocationCreateDto
    {
        [Required(ErrorMessage = "Location name is required")] // Requirement 5: Validation
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [Range(-90, 90)] // Validates geographic boundaries
        public decimal Latitude { get; set; }

        [Required]
        [Range(-180, 180)]
        public decimal Longitude { get; set; }

        public string? Description { get; set; }
    }

    // For updating existing data (PUT)
    public class LocationUpdateDto : LocationCreateDto
    {
        [Required]
        public int Id { get; set; }
    }
}