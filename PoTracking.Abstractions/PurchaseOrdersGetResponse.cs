using POTracking.Domain;
using POTracking.Domain.Dtos;

namespace PoTracking.Abstractions
{
    public class PurchaseOrdersGetResponse : ApiResponse
    {
        public List<PurchaseOrder> PurchaseOrders { get; set; } = new();
    }
}