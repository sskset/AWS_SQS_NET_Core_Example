using System;

namespace AWS_SQS_NET_Core_Example.Database.Entities
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
