using POTracking.Domain;
using static POTracking.Domain.Enums;

namespace POTracking.Tests.Domain
{
    [TestClass]
    public sealed class PurchaseOrderTests
    {
        [TestMethod]
        [DataRow(PurchaseOrderStatus.Pending, PurchaseOrderStatus.Confirmed, true)]
        [DataRow(PurchaseOrderStatus.Pending, PurchaseOrderStatus.Cancelled, true)]
        [DataRow(PurchaseOrderStatus.Pending, PurchaseOrderStatus.Pending, false)]
        [DataRow(PurchaseOrderStatus.Pending, PurchaseOrderStatus.Shipped, false)]
        [DataRow(PurchaseOrderStatus.Pending, PurchaseOrderStatus.Fulfilled, false)]
        public void CanStatusMoveTo_FromPending(PurchaseOrderStatus source, PurchaseOrderStatus target, bool expected)
        {
            var purchaseOrder = new PurchaseOrder { Status = source };

            Assert.AreEqual(expected, purchaseOrder.CanStatusMoveTo(target));
        }

        [TestMethod]
        [DataRow(PurchaseOrderStatus.Confirmed, PurchaseOrderStatus.Shipped, true)]
        [DataRow(PurchaseOrderStatus.Confirmed, PurchaseOrderStatus.Cancelled, true)]
        [DataRow(PurchaseOrderStatus.Confirmed, PurchaseOrderStatus.Pending, false)]
        [DataRow(PurchaseOrderStatus.Confirmed, PurchaseOrderStatus.Confirmed, false)]
        [DataRow(PurchaseOrderStatus.Confirmed, PurchaseOrderStatus.Fulfilled, false)]
        public void CanStatusMoveTo_FromConfirmed(PurchaseOrderStatus source, PurchaseOrderStatus target, bool expected)
        {
            var purchaseOrder = new PurchaseOrder { Status = source };

            Assert.AreEqual(expected, purchaseOrder.CanStatusMoveTo(target));
        }

        [TestMethod]
        [DataRow(PurchaseOrderStatus.Shipped, PurchaseOrderStatus.Fulfilled, true)]
        [DataRow(PurchaseOrderStatus.Shipped, PurchaseOrderStatus.Pending, false)]
        [DataRow(PurchaseOrderStatus.Shipped, PurchaseOrderStatus.Confirmed, false)]
        [DataRow(PurchaseOrderStatus.Shipped, PurchaseOrderStatus.Shipped, false)]
        [DataRow(PurchaseOrderStatus.Shipped, PurchaseOrderStatus.Cancelled, false)]
        public void CanStatusMoveTo_FromShipped(PurchaseOrderStatus source, PurchaseOrderStatus target, bool expected)
        {
            var purchaseOrder = new PurchaseOrder { Status = source };

            Assert.AreEqual(expected, purchaseOrder.CanStatusMoveTo(target));
        }

        [TestMethod]
        [DataRow(PurchaseOrderStatus.Fulfilled, PurchaseOrderStatus.Pending, false)]
        [DataRow(PurchaseOrderStatus.Fulfilled, PurchaseOrderStatus.Confirmed, false)]
        [DataRow(PurchaseOrderStatus.Fulfilled, PurchaseOrderStatus.Shipped, false)]
        [DataRow(PurchaseOrderStatus.Fulfilled, PurchaseOrderStatus.Fulfilled, false)]
        [DataRow(PurchaseOrderStatus.Fulfilled, PurchaseOrderStatus.Cancelled, false)]
        public void CanStatusMoveTo_FromFulfilled(PurchaseOrderStatus source, PurchaseOrderStatus target, bool expected)
        {
            var purchaseOrder = new PurchaseOrder { Status = source };

            Assert.AreEqual(expected, purchaseOrder.CanStatusMoveTo(target));
        }

        [TestMethod]
        [DataRow(PurchaseOrderStatus.Cancelled, PurchaseOrderStatus.Pending, false)]
        [DataRow(PurchaseOrderStatus.Cancelled, PurchaseOrderStatus.Confirmed, false)]
        [DataRow(PurchaseOrderStatus.Cancelled, PurchaseOrderStatus.Shipped, false)]
        [DataRow(PurchaseOrderStatus.Cancelled, PurchaseOrderStatus.Fulfilled, false)]
        [DataRow(PurchaseOrderStatus.Cancelled, PurchaseOrderStatus.Cancelled, false)]
        public void CanStatusMoveTo_FromCancelled(PurchaseOrderStatus source, PurchaseOrderStatus target, bool expected)
        {
            var purchaseOrder = new PurchaseOrder { Status = source };

            Assert.AreEqual(expected, purchaseOrder.CanStatusMoveTo(target));
        }

    }
}
