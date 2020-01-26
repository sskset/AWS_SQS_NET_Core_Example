using AWS_SQS_NET_Core_Example.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AWS_SQS_NET_Core_Example.Database
{
    public interface ICustomerRepository
    {
        Task<Customer> CreateCustomerAsync(string firstName, string lastName);
    }
    public class CustomerRepository : ICustomerRepository
    {
        public Task<Customer> CreateCustomerAsync(string firstName, string lastName)
        {
            return Task.FromResult(new Customer
            {
                FirstName = firstName,
                LastName = lastName,
                Id = Guid.NewGuid()
            });
        }
    }
}
