using PoTracking.Abstractions;
using POTracking.Domain;
using POTracking.Domain.Dtos;

namespace POTracking.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly ICustomerService _customerService;

        public PurchaseOrderService(IPurchaseOrderRepository purchaseOrderRepository, ICustomerService customerService)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _customerService = customerService;
        }

        public PurchaseOrderCreateResponse Create(PurchaseOrderCreateRequest request)
        {
            var errors = Validate(request);
            if (errors.Count > 0)
            {
                return new PurchaseOrderCreateResponse
                {
                    Success = false,
                    Errors = errors
                };
            }

            var purchaseOrder = new PurchaseOrder
            {
                CustomerId = request.CustomerId,
                ExternalRef = request.ExternalRef,
                Status = Enums.PurchaseOrderStatus.Pending,
                Lines = request.Lines
                    .Select(l => new PurchaseOrderLine()
                    {
                        Sku = l.Sku,
                        ProductName = l.ProductName,
                        Quantity = l.Quantity,
                        UnitPrice = l.UnitPrice
                    })
                    .ToList()
            };

            if (!_purchaseOrderRepository.Create(purchaseOrder))
            {
                return new PurchaseOrderCreateResponse
                {
                    Success = false,
                    Errors = new List<string> { $"An order with external reference '{request.ExternalRef}' already exists." }
                };
            }

            return new PurchaseOrderCreateResponse
            {
                Success = true,
                PurchaseOrder = purchaseOrder
            };
        }

        public PurchaseOrderGetResponse GetByNumber(string number)
        {
            var po = _purchaseOrderRepository.GetByNumber(number);
            if (po == null)
            {
                return new PurchaseOrderGetResponse
                {
                    Success = false,
                    Errors = new List<string> { $"Purchase order with number '{number}' not found." }
                };
            }


            return new PurchaseOrderGetResponse
            {
                Success = true,
                PurchaseOrder = po
            };
        }


        public PurchaseOrderStatusUpdateResponse ChangeStatus(string number, Enums.PurchaseOrderStatus status)
        {
            var po = _purchaseOrderRepository.GetByNumber(number);
            if (po == null)
            {
                return new PurchaseOrderStatusUpdateResponse
                {
                    Success = false,
                    Errors = [$"Purchase order with number '{number}' not found."]
                };
            }

            if (!po.CanStatusMoveTo(status))
            {
                return new PurchaseOrderStatusUpdateResponse
                {
                    Success = false,
                    Errors = [$"Cannot move purchase order '{number}' to status '{status}'."]
                };
            }

            po.Status = status;
            return new PurchaseOrderStatusUpdateResponse { Success = true };
        }

        public PurchaseOrdersGetResponse GetAll()
        {
            var orders = _purchaseOrderRepository.GetAll();
            var customerNames = _customerService.GetAll().ToDictionary(c => c.Id, c => c.Name);

            return new PurchaseOrdersGetResponse
            {
                Success = true,
                PurchaseOrders = orders.Select(po => new PurchaseOrderListDto
                {
                    Number = po.Number,
                    ExternalRef = po.ExternalRef,
                    CustomerName = customerNames.GetValueOrDefault(po.CustomerId, string.Empty),
                    Total = po.Total
                }).ToList()
            };
        }

        private List<string> Validate(PurchaseOrderCreateRequest request)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(request.ExternalRef))
                errors.Add("External reference is required.");

            if (!_customerService.Exist(request.CustomerId))
                errors.Add($"Customer with id {request.CustomerId} does not exist.");

            foreach (var line in request.Lines)
            {
                if (string.IsNullOrWhiteSpace(line.Sku))
                    errors.Add("SKU is required.");

                if (string.IsNullOrWhiteSpace(line.ProductName))
                    errors.Add("Product name is required.");

                if (line.Quantity <= 0)
                    errors.Add($"Quantity must be greater than 0. ({line.Sku})");

                if (line.UnitPrice < 0)
                    errors.Add($"Unit price cannot be less than 0. ({line.Sku})");
            }

            return errors;
        }
    }
}