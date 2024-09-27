using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Role : BaseEntity
    
    {    [Required]
         public string Name { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
