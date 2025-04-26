namespace WebApplication1.Models
{

    public class MpesaCallback
    {
        public Body Body { get; set; }
    }

    public class Body
    {
        public StkCallback stkCallback { get; set; }
    }

    public class StkCallback
    {
        public string MerchantRequestID { get; set; }
        public string CheckoutRequestID { get; set; }
        public int ResultCode { get; set; }
        public string ResultDesc { get; set; }
        public CallbackMetadata CallbackMetadata { get; set; }
    }

    public class CallbackMetadata
    {
        public List<CallbackItem> Item { get; set; }
    }

    public class CallbackItem
    {
        public string Name { get; set; }
        public dynamic Value { get; set; }
    }

}
