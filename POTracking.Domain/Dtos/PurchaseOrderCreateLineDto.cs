using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace POTracking.Domain.Dtos
{
    public class PurchaseOrderCreateLineDto
    {
        [Required]
        public string Sku { get; set; }
        [Required]
        public string ProductName { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Unit price cannot be less than 0")]
        public double UnitPrice { get; set; }
    }
}