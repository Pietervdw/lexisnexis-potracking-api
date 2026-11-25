namespace POTracking.Domain.Dtos
{
    public class PurchaseOrdersGetResponse : ApiResponse
    {
        public List<PurchaseOrderListDto> PurchaseOrders { get; set; } = new();
    }
}