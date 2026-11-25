namespace POTracking.Domain.Dtos
{
    public class PurchaseOrdersGetResponse : ApiResponse
    {
        public List<PurchaseOrder> PurchaseOrders { get; set; } = new();
    }
}