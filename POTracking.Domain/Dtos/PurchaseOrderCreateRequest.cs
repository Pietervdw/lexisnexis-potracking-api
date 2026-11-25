
namespace POTracking.Domain.Dtos
{
    public class PurchaseOrderCreateRequest
    {
        public required int CustomerId { get; set; }
        public required string ExternalRef { get; set; }

        public IEnumerable<PurchaseOrderCreateLineDto> Lines { get; set; } = new List<PurchaseOrderCreateLineDto>();
    }
}