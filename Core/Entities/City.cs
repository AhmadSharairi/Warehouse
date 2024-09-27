using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class City : BaseEntity
    {
        [Required]
        public string Name { get; set; } 
        
        [Required]  
        public int CountryId { get; set; }
        public Country Country { get; set; }


      public ICollection<Warehouse> Warehouses { get; set; } = new List<Warehouse>();
    }
}
