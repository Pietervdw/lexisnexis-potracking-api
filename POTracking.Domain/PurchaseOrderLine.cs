namespace POTracking.Domain
{
    public class PurchaseOrderLine
    {
        public string Sku { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }

        public double LineTotal => Quantity * UnitPrice;
    }
}