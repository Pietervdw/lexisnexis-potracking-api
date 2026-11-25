using POTracking.Domain;
using POTracking.Domain.Dtos;

namespace PoTracking.Abstractions
{
    public interface IPurchaseOrderService
    {
        PurchaseOrderCreateResponse Create(PurchaseOrderCreateRequest request);
        PurchaseOrderGetResponse GetByNumber(string number);
        PurchaseOrdersGetResponse GetAll();
        PurchaseOrderStatusUpdateResponse ChangeStatus(string number, Enums.PurchaseOrderStatus status);
    }
}