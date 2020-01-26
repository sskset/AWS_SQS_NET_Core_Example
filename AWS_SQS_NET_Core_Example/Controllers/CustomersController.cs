using AWS_SQS_NET_Core_Example.Core.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AWS_SQS_NET_Core_Example.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateCustomerCommand payload)
        {

            await _mediator.Send(payload);
            return Ok();
        }
    }
}
