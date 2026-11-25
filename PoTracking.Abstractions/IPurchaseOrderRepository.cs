using POTracking.Domain;

namespace PoTracking.Abstractions
{
    public interface IPurchaseOrderRepository
    {
        bool Create(PurchaseOrder purchaseOrder);
    }
}
