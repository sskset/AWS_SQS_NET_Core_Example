using Amazon.SQS;
using Amazon.SQS.Model;
using AWS_SQS_NET_Core_Example.Core.Messages;
using MediatR;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AWS_SQS_NET_Core_Example.HostedServices
{
    public class Worker : BackgroundService
    {
        private readonly IAmazonSQS _amazonSQS;
        private readonly AWSSQSHelper _helper;
        private readonly IMediator _mediator;

        public Worker(IAmazonSQS amazonSQS, AWSSQSHelper helper, IMediator mediator)
        {
            _amazonSQS = amazonSQS ?? throw new ArgumentNullException(nameof(amazonSQS));
            _helper = helper ?? throw new ArgumentNullException(nameof(helper));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var queueUrl = await _amazonSQS.GetQueueUrlAsync(new GetQueueUrlRequest { QueueName = "test_customer_created.fifo" }, stoppingToken);

            CreateQueueResponse createQueueResponse = null;
            if (queueUrl == null || queueUrl.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                createQueueResponse = await _helper.CreateQueueAsync(new CreateQueueRequest { QueueName = "test_customer_created.fifo" }, stoppingToken);
            }
            else
            {
                createQueueResponse = new CreateQueueResponse
                {
                    QueueUrl = queueUrl.QueueUrl
                };
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                var req = new ReceiveMessageRequest
                {
                    QueueUrl = createQueueResponse.QueueUrl,
                    MaxNumberOfMessages = 5,
                    WaitTimeSeconds = 5
                };

                var result = await _amazonSQS.ReceiveMessageAsync(req);

                if (result.Messages.Any())
                {
                    foreach (var msg in result.Messages)
                    {
                        var @event = JsonConvert.DeserializeObject<CustomerCreatedEvent>(msg.Body);

                        //consumer @event
                        await _mediator.Send(@event);

                        var deleteMessageRequest = new DeleteMessageRequest
                        {
                            QueueUrl = createQueueResponse.QueueUrl,
                            ReceiptHandle = msg.ReceiptHandle
                        };

                        var deleteResponse = await _amazonSQS.DeleteMessageAsync(deleteMessageRequest, stoppingToken);

                        Console.WriteLine(JsonConvert.SerializeObject(deleteResponse));
                    }
                }

            }
        }
    }
}
