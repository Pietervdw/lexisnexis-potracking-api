using static POTracking.Domain.Enums;

namespace POTracking.Domain
{
    public class PurchaseOrder
    {
        public string Number { get; set; }
        public string ExternalRef { get; set; }
        public double DeliveryFee { get; set; }
        public double TaxPercentage { get; set; } = 15;

        public PurchaseOrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public IEnumerable<PurchaseOrderLine> Lines { get; set; } = new List<PurchaseOrderLine>();

        public double TaxAmount => Subtotal * (TaxPercentage / 100);
        public double Subtotal => Lines.Sum(line => line.LineTotal);
        public double Total => Subtotal + DeliveryFee + TaxAmount;

        public bool CanStatusMoveTo(PurchaseOrderStatus targetStatus)
        {
            switch (Status)
            {
                case PurchaseOrderStatus.Pending:
                    return targetStatus == PurchaseOrderStatus.Confirmed || targetStatus == PurchaseOrderStatus.Cancelled;
                case PurchaseOrderStatus.Confirmed:
                    return targetStatus == PurchaseOrderStatus.Shipped || targetStatus == PurchaseOrderStatus.Cancelled;
                case PurchaseOrderStatus.Shipped:
                    return targetStatus == PurchaseOrderStatus.Fulfilled;
                case PurchaseOrderStatus.Fulfilled:
                    return false; // Once fulfilled it can't move to another status.
                case PurchaseOrderStatus.Cancelled:
                    return false; // Same as fulgilled, cant move once cancelled.
                default:
                    return false;
            }
        }
    }
}