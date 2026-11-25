using PoTracking.Abstractions;
using POTracking.Domain;

namespace POTracking.Data.InMemory
{
    public class CustomerRepository : ICustomerRepository
    {
        public IEnumerable<Customer> GetAll()
        {
            return new List<Customer>
            {
                new Customer { Id = 1, Name = "Acme Industries", Email = "purchasing@acme.com" },
                new Customer { Id = 2, Name = "Globex Corporation", Email = "orders@globex.com" },
                new Customer { Id = 3, Name = "Initech", Email = "procurement@initech.com" },
                new Customer { Id = 4, Name = "Umbrella Logistics", Email = "buy@umbrellalogistics.com" },
                new Customer { Id = 5, Name = "Stark Manufacturing", Email = "supply@starkmfg.com" }
            };
        }

        public bool Exist(int customerId)
        {
            return GetAll().Any(c => c.Id == customerId);
        }
    }
}