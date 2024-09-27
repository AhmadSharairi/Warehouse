using System.ComponentModel.DataAnnotations;
using Core.Entities;

public class Country : BaseEntity
{
    [Required]
    public string Name { get; set; }  
    public ICollection<City> Cities { get; set; }  = new List<City>();
}
