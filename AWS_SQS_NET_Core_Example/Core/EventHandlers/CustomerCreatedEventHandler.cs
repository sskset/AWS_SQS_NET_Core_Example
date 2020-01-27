using AWS_SQS_NET_Core_Example.Core.Messages;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AWS_SQS_NET_Core_Example.Core.EventHandlers
{
    public class CustomerCreatedEventHandler : IRequestHandler<CustomerCreatedEvent>
    {
        public CustomerCreatedEventHandler()
        {
        }

        public Task<Unit> Handle(CustomerCreatedEvent request, CancellationToken cancellationToken)
        {
            // DO SOME WORK AT HERE REGARDS TO THE EVENT

            return Task.FromResult(Unit.Value);
        }
    }
}
