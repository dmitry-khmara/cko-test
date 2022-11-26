using CKO.PaymentGateway.Api.Controllers.CreatePayment;
using CKO.PaymentGateway.Api.Controllers.GetPayment;
using CKO.PaymentGateway.Application.CreatePayment;
using CKO.PaymentGateway.Application.GetPayment;
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

        [HttpGet("{paymentId}")]
        public async Task<IActionResult> GetPayment(Guid paymentId, [FromQuery] Guid merchantId)
        {

            var payment = await _mediator.Send(new GetPaymentQuery(paymentId, merchantId));

            if (payment == null)
                return NotFound();

            return Ok(new GetPaymentResponse(payment));
        }
    }
}