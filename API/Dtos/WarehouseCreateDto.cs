using System.ComponentModel.DataAnnotations;

namespace Core.DTOs
{
    public class WarehouseCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }


        [Required]
        [MaxLength(200)]  
        public string Address { get; set; }

        [Required]
        [MaxLength(100)] 
        public string CityName { get; set; }

        [Required]
        [MaxLength(100)] 
        public string CountryName { get; set; }
    }
}
