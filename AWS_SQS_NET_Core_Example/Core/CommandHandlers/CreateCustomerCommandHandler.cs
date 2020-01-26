using Amazon.SimpleNotificationService;
using Amazon.SQS;
using Amazon.SQS.Model;
using AWS_SQS_NET_Core_Example.Core.Commands;
using AWS_SQS_NET_Core_Example.Core.Messages;
using AWS_SQS_NET_Core_Example.Database;
using AWS_SQS_NET_Core_Example.Database.Entities;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AWS_SQS_NET_Core_Example.Core.CommandHandlers
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Customer>
    {
        //private readonly IAmazonSimpleNotificationService _amazonSimpleNotificationService;
        private readonly ICustomerRepository _customerRepository;
        private readonly IAmazonSQS _amazonSQS;
        private readonly AWSSQSHelper _helper;

        //public CreateCustomerCommandHandler(IAmazonSimpleNotificationService amazonSimpleNotificationService, ICustomerRepository customerRepository)
        //{
        //    _amazonSimpleNotificationService = amazonSimpleNotificationService ?? throw new ArgumentNullException(nameof(amazonSimpleNotificationService));
        //    _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        //}

        public CreateCustomerCommandHandler(IAmazonSQS amazonSQS, ICustomerRepository customerRepository, AWSSQSHelper helper)
        {
            _amazonSQS = amazonSQS ?? throw new ArgumentNullException(nameof(amazonSQS));
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _helper = helper ?? throw new ArgumentNullException(nameof(helper));
        }

        //public async Task<Customer> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        //{
        //    var customerCreated = await _customerRepository.CreateCustomerAsync(request.FirstName, request.LastName);

        //    var createTopicResponse = await _amazonSimpleNotificationService.CreateTopicAsync(nameof(CreateCustomerCommand));

        //    var response = await _amazonSimpleNotificationService.PublishAsync(createTopicResponse.TopicArn, JsonConvert.SerializeObject(customerCreated));

        //    return customerCreated;
        //}

        public async Task<Customer> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customerCreated = await _customerRepository.CreateCustomerAsync(request.FirstName, request.LastName);

            var queueUrl = await _amazonSQS.GetQueueUrlAsync(new GetQueueUrlRequest { QueueName = "test_customer_created.fifo" }, cancellationToken);

            CreateQueueResponse createQueueResponse;
            if (queueUrl == null || queueUrl.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                createQueueResponse = await _helper.CreateQueueAsync(new CreateQueueRequest { QueueName = "test_customer_created.fifo" }, cancellationToken);
            }
            else
            {
                createQueueResponse = new CreateQueueResponse
                {
                    QueueUrl = queueUrl.QueueUrl
                };
            }

            var response = await _amazonSQS.SendMessageAsync(
                new SendMessageRequest { 
                    MessageBody = JsonConvert.SerializeObject(new CustomerCreatedEvent() { Customer = customerCreated }), 
                    QueueUrl = createQueueResponse.QueueUrl , MessageGroupId = "CustomerCreated", MessageDeduplicationId=$"CustomerCreated:{customerCreated.Id}"}, cancellationToken);

            return customerCreated;
        }
    }
}
