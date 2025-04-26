using Microsoft.AspNetCore.Mvc;
using WebApplication1;

namespace WebApplication1
{
    [ApiController]
    [Route("api/[controller]")]
    public class MpesaCallbackController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> ReceiveCallback([FromBody] MpesaCallback callback)
        {
            var stkCallback = callback.Body.stkCallback;

            string merchantRequestId = stkCallback.MerchantRequestID;
            string checkoutRequestId = stkCallback.CheckoutRequestID;
            int resultCode = stkCallback.ResultCode;
            string resultDesc = stkCallback.ResultDesc;

            string mpesaReceiptNumber = "";
            decimal amount = 0;
            string phoneNumber = "";
            string transactionDate = "";

            if (stkCallback.CallbackMetadata?.Item != null)
            {
                foreach (var item in stkCallback.CallbackMetadata.Item)
                {
                    if (item.Name == "MpesaReceiptNumber")
                        mpesaReceiptNumber = item.Value;
                    if (item.Name == "Amount")
                        amount = item.Value;
                    if (item.Name == "PhoneNumber")
                        phoneNumber = item.Value;
                    if (item.Name == "TransactionDate")
                        transactionDate = item.Value;
                }
            }

            // For now, just log to console (or save to DB later)
            Console.WriteLine($"MerchantRequestID: {merchantRequestId}");
            Console.WriteLine($"CheckoutRequestID: {checkoutRequestId}");
            Console.WriteLine($"ResultCode: {resultCode}");
            Console.WriteLine($"ResultDesc: {resultDesc}");
            Console.WriteLine($"MpesaReceiptNumber: {mpesaReceiptNumber}");
            Console.WriteLine($"Amount: {amount}");
            Console.WriteLine($"PhoneNumber: {phoneNumber}");
            Console.WriteLine($"TransactionDate: {transactionDate}");

            // Always return 200 OK
            return Ok();
        }
    }
}
