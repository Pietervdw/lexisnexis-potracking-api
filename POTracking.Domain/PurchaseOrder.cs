
using static POTracking.Domain.Enums;

namespace POTracking.Domain
{
    public class PurchaseOrder
    {
        public int CustomerId { get; set; }
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

        private static readonly IReadOnlyDictionary<PurchaseOrderStatus, PurchaseOrderStatus[]> AllowedStatusTransitions = new Dictionary<PurchaseOrderStatus, PurchaseOrderStatus[]>
        {
            [PurchaseOrderStatus.Pending] = [PurchaseOrderStatus.Confirmed, PurchaseOrderStatus.Cancelled],
            [PurchaseOrderStatus.Confirmed] = [PurchaseOrderStatus.Shipped, PurchaseOrderStatus.Cancelled],
            [PurchaseOrderStatus.Shipped] = [PurchaseOrderStatus.Fulfilled],
            [PurchaseOrderStatus.Fulfilled] = [],
            [PurchaseOrderStatus.Cancelled] = []
        };

        public bool CanStatusMoveTo(PurchaseOrderStatus targetStatus)
        {
            var allowed = AllowedStatusTransitions.TryGetValue(Status, out var transitions) ? transitions : [];
            return allowed.Contains(targetStatus);
        }
    }
}