using PoTracking.Abstractions;
using POTracking.Domain;

namespace POTracking.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public IEnumerable<Customer> GetAll()
        {
            return _customerRepository.GetAll();
        }

        public bool Exist(int customerId)
        {
            return _customerRepository.Exist(customerId);
        }
    }
}
