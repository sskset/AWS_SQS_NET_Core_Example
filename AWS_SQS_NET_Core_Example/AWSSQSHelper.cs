using Amazon.SQS;
using Amazon.SQS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AWS_SQS_NET_Core_Example
{
    public class AWSSQSHelper
    {
        private readonly IAmazonSQS _amazonSQS;

        public AWSSQSHelper(IAmazonSQS amazonSQS)
        {
            _amazonSQS = amazonSQS ?? throw new ArgumentNullException(nameof(amazonSQS));
        }

        public async Task<CreateQueueResponse> CreateQueueAsync(CreateQueueRequest request, CancellationToken cancellationToken = default)
        {
            int maxMessage = 256 * 1024;
            var attrs = new Dictionary<string, string>();
            attrs.Add(QueueAttributeName.MaximumMessageSize, maxMessage.ToString());
            attrs.Add(QueueAttributeName.VisibilityTimeout, "10");
            attrs.Add(QueueAttributeName.FifoQueue, "true");

            request.Attributes = attrs;

            return await _amazonSQS.CreateQueueAsync(request, cancellationToken);
        }
    }
}
