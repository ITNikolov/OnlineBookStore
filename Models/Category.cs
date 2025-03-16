using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace OnlineBookStore.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Category Name is required.")]
        [MaxLength(30, ErrorMessage = "Category Name cannot exceed 30 characters.")]
        [DisplayName("Category Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Display Order is required.")]
        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "Display Order must be between 1-100.")]
        public int DisplayOrder { get; set; }
    }
}

