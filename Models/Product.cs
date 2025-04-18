using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineBookStore.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Product Title is required.")]
        [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string Title { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; } // Optional, but limited to 500 chars.

        [Required(ErrorMessage = "ISBN is required.")]
        [MaxLength(20, ErrorMessage = "ISBN cannot exceed 20 characters.")]
        public string ISBN { get; set; } = string.Empty;

        [Required(ErrorMessage = "Author is required.")]
        [MaxLength(50, ErrorMessage = "Author name cannot exceed 50 characters.")]
        public string Author { get; set; } = string.Empty;

        [Required(ErrorMessage = "List Price is required.")]
        [Display(Name = "List Price")]
        [Range(1, 1000, ErrorMessage = "List Price must be between 1 and 1000.")]
        public double ListPrice { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }

        [Required(ErrorMessage = "Image is required.")]
        public string ImageURL { get; set; } = string.Empty;
    }
}
