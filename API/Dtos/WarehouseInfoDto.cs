namespace Core.DTOs
{
    public class WarehouseInfoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public int ItemsCount { get; set; }
        public string Status { get; set; }
    }
}
