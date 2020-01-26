using AWS_SQS_NET_Core_Example.Database.Entities;
using MediatR;

namespace AWS_SQS_NET_Core_Example.Core.Commands
{
    public class CreateCustomerCommand : IRequest<Customer>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public CreateCustomerCommand(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public CreateCustomerCommand()
        {

        }
    }
}
