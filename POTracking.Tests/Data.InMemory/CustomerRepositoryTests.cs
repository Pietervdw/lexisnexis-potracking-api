using POTracking.Data.InMemory;
namespace POTracking.Tests.Data.InMemory
{
    [TestClass]
    public sealed class CustomerRepositoryTests
    {
        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        [DataRow(5)]
        public void Exist_AllCustomersExist_ReturnsTrue(int customerId)
        {
            var repository = new CustomerRepository();

            Assert.IsTrue(repository.Exist(customerId));
        }

        [TestMethod]
        [DataRow(6)]
        [DataRow(-1)]
        [DataRow(int.MaxValue)]
        public void Exist_CustomersDoesNotExist_ReturnsFalse(int customerId)
        {
            var repository = new CustomerRepository();

            Assert.IsFalse(repository.Exist(customerId));
        }

        [TestMethod]
        public void GetAll_ReturnsAllCustomers()
        {
            var repository = new CustomerRepository();

            Assert.HasCount(5, repository.GetAll().ToList());
        }
      
    }
}
