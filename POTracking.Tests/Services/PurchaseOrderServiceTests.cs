using Moq;
using PoTracking.Abstractions;
using POTracking.Domain;
using POTracking.Domain.Dtos;
using POTracking.Services;
using static POTracking.Domain.Enums;

namespace POTracking.Tests.Services
{
    [TestClass]
    public sealed class PurchaseOrderServiceTests
    {

        private static PurchaseOrderCreateRequest BuildRequest()
        {
            return new PurchaseOrderCreateRequest
            {
                CustomerId = 1,
                ExternalRef = "EXT-001",
                Lines =
                [
                    new PurchaseOrderCreateLineDto { Sku = "SKU-1", ProductName = "Widget", Quantity = 2, UnitPrice = 10.50 },
                    new PurchaseOrderCreateLineDto { Sku = "SKU-2", ProductName = "Gadget", Quantity = 1, UnitPrice = 5.0 }
                ]
            };
        }

        private static Mock<IPurchaseOrderRepository> CreateRepositoryMock()
        {
            var mock = new Mock<IPurchaseOrderRepository>();
            // The repo creates the Number, so we assign it here.
            mock.Setup(r => r.Create(It.IsAny<PurchaseOrder>()))
                .Callback((PurchaseOrder po) => po.Number = "PO0001")
                .Returns(true);
            return mock;
        }

        private static PurchaseOrderService BuildService(
            Mock<IPurchaseOrderRepository> repositoryMock,
            Mock<ICustomerService> customerServiceMock)
        {
            return new PurchaseOrderService(repositoryMock.Object, customerServiceMock.Object);
        }

        [TestMethod]
        public void Create_ValidRequest_ReturnsSuccessWithPendingOrderMappedLinesAndNumber()
        {
            var repositoryMock = CreateRepositoryMock();
            var customerServiceMock = new Mock<ICustomerService>();
            customerServiceMock.Setup(s => s.Exist(1)).Returns(true);
            var service = BuildService(repositoryMock, customerServiceMock);

            var response = service.Create(BuildRequest());

            Assert.IsTrue(response.Success);
            Assert.IsNull(response.Errors);
            Assert.IsNotNull(response.PurchaseOrder);
            Assert.AreEqual("PO0001", response.PurchaseOrder.Number);
            Assert.AreEqual(PurchaseOrderStatus.Pending, response.PurchaseOrder.Status);
            Assert.AreEqual("EXT-001", response.PurchaseOrder.ExternalRef);
            var lines = response.PurchaseOrder.Lines.ToList();
            Assert.HasCount(2, lines);
            Assert.AreEqual("SKU-1", lines[0].Sku);
            Assert.AreEqual("Widget", lines[0].ProductName);
            Assert.AreEqual(2, lines[0].Quantity);
            Assert.AreEqual(10.50, lines[0].UnitPrice);
            Assert.AreEqual("SKU-2", lines[1].Sku);
            Assert.AreEqual("Gadget", lines[1].ProductName);
            Assert.AreEqual(1, lines[1].Quantity);
            Assert.AreEqual(5.0, lines[1].UnitPrice);
            customerServiceMock.Verify(s => s.Exist(1), Times.Once);
            repositoryMock.Verify(r => r.Create(It.IsAny<PurchaseOrder>()), Times.Once);
        }

        [TestMethod]
        public void Create_ValidRequest_CapturedEntityMatchesRequest()
        {
            PurchaseOrder? captured = null;
            var repositoryMock = new Mock<IPurchaseOrderRepository>();
            repositoryMock.Setup(r => r.Create(It.IsAny<PurchaseOrder>()))
                .Callback((PurchaseOrder po) => captured = po)
                .Returns(true);
            var customerServiceMock = new Mock<ICustomerService>();
            customerServiceMock.Setup(s => s.Exist(1)).Returns(true);
            var service = BuildService(repositoryMock, customerServiceMock);
            var request = BuildRequest();

            service.Create(request);

            Assert.IsNotNull(captured);
            Assert.AreEqual(PurchaseOrderStatus.Pending, captured.Status);
            Assert.AreEqual("EXT-001", captured.ExternalRef);
            var lines = captured.Lines.ToList();
            Assert.HasCount(2, lines);
            Assert.AreEqual("SKU-1", lines[0].Sku);
            Assert.AreEqual("Widget", lines[0].ProductName);
            Assert.AreEqual(2, lines[0].Quantity);
            Assert.AreEqual(10.50, lines[0].UnitPrice);
            Assert.AreEqual("SKU-2", lines[1].Sku);
            Assert.AreEqual(5.0, lines[1].UnitPrice);
            Assert.AreEqual(15, captured.TaxPercentage);
        }

        [TestMethod]
        public void Create_EmptyExternalRef_ReturnsExternalRefRequiredError()
        {
            var repositoryMock = CreateRepositoryMock();
            var customerServiceMock = new Mock<ICustomerService>();
            customerServiceMock.Setup(s => s.Exist(1)).Returns(true);
            var service = BuildService(repositoryMock, customerServiceMock);
            var request = BuildRequest();
            request.ExternalRef = "";

            var response = service.Create(request);

            Assert.IsFalse(response.Success);
            Assert.IsNull(response.PurchaseOrder);
            var errors = response.Errors!.ToList();
            Assert.HasCount(1, errors);
            Assert.AreEqual("External reference is required.", errors[0]);
            repositoryMock.Verify(r => r.Create(It.IsAny<PurchaseOrder>()), Times.Never);
        }
        
        [TestMethod]
        public void Create_UnknownCustomer_ReturnsCustomerDoesNotExistError()
        {
            var repositoryMock = CreateRepositoryMock();
            var customerServiceMock = new Mock<ICustomerService>();
            customerServiceMock.Setup(s => s.Exist(999)).Returns(false);
            var service = BuildService(repositoryMock, customerServiceMock);
            var request = BuildRequest();
            request.CustomerId = 999;

            var response = service.Create(request);

            Assert.IsFalse(response.Success);
            Assert.IsNull(response.PurchaseOrder);
            var errors = response.Errors!.ToList();
            Assert.HasCount(1, errors);
            Assert.AreEqual("Customer with id 999 does not exist.", errors[0]);
            repositoryMock.Verify(r => r.Create(It.IsAny<PurchaseOrder>()), Times.Never);
        }
    }
}
