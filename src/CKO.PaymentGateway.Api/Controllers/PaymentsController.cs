using CKO.PaymentGateway.Api.Controllers.CreatePayment;
using CKO.PaymentGateway.Application.CreatePaymentRequest;
using CKO.PaymentGateway.Domain.Cards;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CKO.PaymentGateway.Api.Controllers
{
    [ApiController]
    [Route("payments")]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CreatePaymentCommand command;

            try
            {
                command = request.CreateCommand();
            }
            catch (InvalidCardDetailsException e)
            {
                ModelState.AddModelError(e.Field.ToString(), "Invalid card details");
                return BadRequest(ModelState);
            }

            var paymentId = await _mediator.Send(command);

            return Ok(paymentId);
        }
    }
}