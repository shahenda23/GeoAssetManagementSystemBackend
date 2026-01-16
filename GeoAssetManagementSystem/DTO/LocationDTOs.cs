using System.ComponentModel.DataAnnotations;

namespace GeoAssetManagementSystem.DTOs
{
    public class LocationReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string? Description { get; set; }
        public DateTime DateTime { get; set; }
    }

    public class LocationCreateDto
    {
        [Required(ErrorMessage = "Location name is required")] 
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [Range(-90, 90)] 
        public decimal Latitude { get; set; }

        [Required]
        [Range(-180, 180)]
        public decimal Longitude { get; set; }

        public string? Description { get; set; }
    }

    public class LocationUpdateDto : LocationCreateDto
    {
        [Required]
        public int Id { get; set; }
    }
}