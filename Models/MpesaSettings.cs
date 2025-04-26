namespace WebApplication1.Models
{
    public class MpesaSettings
    {
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public string AuthUrl { get; set; }
        public string LipaNaMpesaUrl { get; set; }
        public string B2BUrl { get; set; }          // NEW: B2B Payment URL
        public string Shortcode { get; set; }
        public string Party { get; set; }
        public string PassKey { get; set; }
        public string CallbackUrl { get; set; }
        public string InitiatorName { get; set; }    // NEW: Initiator (API_Username)
        public string SecurityCredential { get; set; } // NEW: Encrypted initiator password
        public string QueueTimeoutUrl { get; set; }  // NEW: Timeout URL
        public string ResultUrl { get; set; }        // NEW: Result URL
        public string Status { get; set; }
        public string ErrorMessage { get; set; }
        public string TransactionId { get; set; }
        public string TransactionDate { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseDescription { get; set; }

    }

}
