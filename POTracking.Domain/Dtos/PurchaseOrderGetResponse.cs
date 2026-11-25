namespace POTracking.Domain.Dtos
{
    public class PurchaseOrderGetResponse : ApiResponse
    {
        public PurchaseOrder? PurchaseOrder { get; set; }
    }
}