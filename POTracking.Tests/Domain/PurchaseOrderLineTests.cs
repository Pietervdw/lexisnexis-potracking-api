using POTracking.Domain;

namespace POTracking.Tests.Domain
{
    [TestClass]
    public sealed class PurchaseOrderLineTests
    {

        [TestMethod]
        public void LineTotal_QuantityTimesUnitPrice_EqualsProduct()
        {
            var line = new PurchaseOrderLine { Quantity = 3, UnitPrice = 19.99 };

            Assert.AreEqual(59.97, line.LineTotal);
        }

        [TestMethod]
        public void LineTotal_ZeroQuantity_IsZero()
        {
            var line = new PurchaseOrderLine { Quantity = 0, UnitPrice = 19.99 };

            Assert.AreEqual(0, line.LineTotal);
        }

        [TestMethod]
        public void LineTotal_ZeroUnitPrice_IsZero()
        {
            var line = new PurchaseOrderLine { Quantity = 3, UnitPrice = 0 };

            Assert.AreEqual(0, line.LineTotal);
        }
    }
}
