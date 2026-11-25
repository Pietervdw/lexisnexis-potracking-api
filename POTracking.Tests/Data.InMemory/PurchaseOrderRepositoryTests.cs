using POTracking.Data.InMemory;
using POTracking.Domain;

namespace POTracking.Tests.Data.InMemory
{
    [TestClass]
    public sealed class PurchaseOrderRepositoryTests
    {
        private static PurchaseOrder BuildOrder(string externalRef)
        {
            return new PurchaseOrder { ExternalRef = externalRef };
        }

        [TestMethod]
        public void Create_FreshRepository_FirstOrderGetsNumberPo0001()
        {
            var repository = new PurchaseOrderRepository();
            var order = BuildOrder("EXT01");

            var result = repository.Create(order);

            Assert.IsTrue(result);
            Assert.AreEqual("PO0001", order.Number);
        }

        [TestMethod]
        public void Create_PoNumberShouldIncrease_GetPo0001ThenPo0002()
        {
            var repository = new PurchaseOrderRepository();
            var first = BuildOrder("EXT01");
            var second = BuildOrder("EXT02");

            Assert.IsTrue(repository.Create(first));
            Assert.IsTrue(repository.Create(second));
            Assert.AreEqual("PO0001", first.Number);
            Assert.AreEqual("PO0002", second.Number);
        }

        [TestMethod]
        public void Create_CheckForDuplicateRef_ReturnsFalse()
        {
            var repository = new PurchaseOrderRepository();
            var original = BuildOrder("EXT01");
            var duplicate = BuildOrder("EXT01");

            Assert.IsTrue(repository.Create(original));
            Assert.IsFalse(repository.Create(duplicate));
        }
     
    }
}
