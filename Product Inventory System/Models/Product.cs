using System.ComponentModel.DataAnnotations;

namespace Product_Inventory_System.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Price must be greater than or equal to 1.")]
        public int Price { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than or equal to 1.")]
        public int Quantity { get; set; }

        [Required]
        public string AddedBy { get; set; }
    }
}
