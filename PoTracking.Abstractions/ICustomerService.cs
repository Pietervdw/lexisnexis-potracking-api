using POTracking.Domain;

namespace PoTracking.Abstractions
{
    public interface ICustomerService
    {
        IEnumerable<Customer> GetAll();
        bool Exist(int customerId);
    }
}
