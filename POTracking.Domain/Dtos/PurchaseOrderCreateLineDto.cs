namespace POTracking.Domain.Dtos
{
    public class PurchaseOrderCreateLineDto
    {
        public string Sku { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
    }
}