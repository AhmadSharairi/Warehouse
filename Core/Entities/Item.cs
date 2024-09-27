using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Item : BaseEntity
    {
     
        [Required]
        public string Name { get; set; }

        [MaxLength(50)]
        public string SKUCode { get; set; }


        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

    
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Cost Price must be greater than 0")]
        public decimal CostPrice { get; set; }

         public decimal? MSRPPrice { get; set; }


        [Required]
        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
    }
}
