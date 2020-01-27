using Amazon.SimpleNotificationService;
using AWS_SQS_NET_Core_Example.Core.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AWS_SQS_NET_Core_Example.Core.CommandHandlers
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand>
    {
        private readonly IAmazonSimpleNotificationService _notificationService;

        public DeleteCustomerCommandHandler(IAmazonSimpleNotificationService notificationService)
        {

        }

        public Task<Unit> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
