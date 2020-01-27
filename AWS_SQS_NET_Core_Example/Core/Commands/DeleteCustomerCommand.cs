using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AWS_SQS_NET_Core_Example.Core.Commands
{
    public class DeleteCustomerCommand : IRequest
    {
        public Guid CustomerId { get; set; }

        public DeleteCustomerCommand()
        {

        }
    }
}
