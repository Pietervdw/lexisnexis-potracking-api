namespace POTracking.Domain.Dtos
{
    public class PurchaseOrderCreateResponse
    {
        public bool Success { get; set; }

        public IEnumerable<string>? Errors { get; set; }
        public PurchaseOrder? PurchaseOrder { get; set; }
    }
}