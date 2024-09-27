namespace API.Dtos
{
   
    public class ItemDto
    {
        public string Name { get; set; }

        public string SKUCode { get; set; }

        public int Quantity { get; set; }

        public decimal CostPrice { get; set; }

         public decimal? MSRPPrice { get; set; }

    }
}


