using FlorApp.Domain.Entities;

namespace FlorApp.Application.Services
{
    public class CustomerService
    {
        private readonly List<Customer> _customers = new();

        public void AddCustomer(string name, string email)
        {
            var customer = new Customer(name, email);
            _customers.Add(customer);
        }

        public List<Customer> GetAllCustomers()
        {
            return _customers;
        }
    }
}
