using AWS_SQS_NET_Core_Example.Database.Entities;
using MediatR;
using System;

namespace AWS_SQS_NET_Core_Example.Core.Messages
{
    public class CustomerCreatedEvent : IRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Customer Customer { get; set; }
        public DateTime OccursOn { get; set; } = DateTime.Now;
    }
}
