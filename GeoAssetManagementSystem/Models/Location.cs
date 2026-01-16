using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeoAssetManagementSystem.Models;

namespace GeoAssetManagementSystem.Models
{
    public class Location
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; } 

        [Required]
        [Column(TypeName = "decimal(18,10)")]
        public decimal Latitude { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,10)")]
        public decimal Longitude { get; set; }

        public string? Description { get; set; }

        public DateTime DateTime { get; set; } = DateTime.UtcNow;

        [ForeignKey("User")]
        public string UserId { get; set; }
        public User? User { get; set; }
    }
}
