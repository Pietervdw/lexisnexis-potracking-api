using System.ComponentModel.DataAnnotations;

namespace POTracking.Domain.Dtos
{
    public class PurchaseOrderCreateRequest
    {
        [Required]
        public required int CustomerId { get; set; }
        [Required(ErrorMessage = "External reference is required.")]
        public required string ExternalRef { get; set; }

        public IEnumerable<PurchaseOrderCreateLineDto> Lines { get; set; } = new List<PurchaseOrderCreateLineDto>();
    }
}