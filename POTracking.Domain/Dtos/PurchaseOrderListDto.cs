namespace POTracking.Domain.Dtos
{
    public class PurchaseOrderListDto
    {
        public string Number { get; set; }
        public string ExternalRef { get; set; }
        public string CustomerName { get; set; }
        public double Total { get; set; }

    }
}