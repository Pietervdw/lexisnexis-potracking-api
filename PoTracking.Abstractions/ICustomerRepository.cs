using POTracking.Domain;

namespace PoTracking.Abstractions
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetAll();
        bool Exist(int customerId);
    }
}