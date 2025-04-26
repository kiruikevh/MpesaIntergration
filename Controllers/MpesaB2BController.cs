using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MpesaB2BController : ControllerBase
    {
        private readonly MpesaB2BService _b2bService;

        public MpesaB2BController(MpesaB2BService b2bService)
        {
            _b2bService = b2bService;
        }

        [HttpPost("pay")]
        public async Task<IActionResult> PayBill(string partyB, string amount, string accountReference, string requesterPhone, string remarks)
        {
            var result = await _b2bService.MakeBusinessPayment(partyB, amount, accountReference, requesterPhone, remarks);
            return Ok(result);
        }
        [HttpPost("queue")]
        public IActionResult QueueTimeout([FromBody] MpesaSettings response)
        {
            // Log or handle timeout
            Console.WriteLine($"Timeout Notification: {response.Status} - {response.ErrorMessage}");
            return Ok(new { message = "Queue Timeout Notification Received" });
        }

        // Handle Result Notification
        [HttpPost("result")]
        public IActionResult Result([FromBody] MpesaSettings result)
        {
            if (result.Status == "Success")
            {
                Console.WriteLine($"Transaction Success: {result.TransactionId}");
            }
            else
            {
                Console.WriteLine($"Transaction Failed: {result.ErrorMessage}");
            }

            return Ok(new { message = "Result Notification Received" });
        }
    }

}
