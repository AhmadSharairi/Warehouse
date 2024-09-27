using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Warehouse : BaseEntity
    {
      
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

 
        [Required]
        [MaxLength(200)]  
        public string Address { get; set; }

        [Required]
        public int CityId { get; set; }
        
        public City City { get; set; }

        [Required]
        public int CountryId { get; set; }

        public Country Country { get; set; }

        // Items collection
        public ICollection<Item> Items { get; set; } = new List<Item>();
    }
}
