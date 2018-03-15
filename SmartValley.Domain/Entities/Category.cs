using System.ComponentModel.DataAnnotations;

namespace SmartValley.Domain.Entities
{
    public class Category
    {
        public CategoryType Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
