using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using WebApplication1.Models;
namespace WebApplication1.Controllers
{
    //get access token
    public class MpesaAuthService
    {
        public readonly MpesaSettings _settings;
        public MpesaAuthService(IOptions<MpesaSettings> settings)
        {
            _settings = settings.Value;
        }
        public async Task<string> GetAccessToken()
        {
            using var client = new HttpClient();
            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_settings.ConsumerKey}:{_settings.ConsumerSecret}"));

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            var response = await client.GetAsync(_settings.AuthUrl);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(content);

            return result.access_token;
        }
    }
    //calling the MpesaAuthService get the token and initiate payment
    public class MpesaStkPushService
    {
        private readonly MpesaSettings _settings;
        private readonly MpesaAuthService _authService;

        public MpesaStkPushService(IOptions<MpesaSettings> settings, MpesaAuthService authService)
        {
            _settings = settings.Value;
            _authService = authService;
        }

        public async Task<string> InitiatePayment(string phoneNumber, string amount, string accountReference, string transactionDesc)
        {
            string accessToken = await _authService.GetAccessToken();

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var password = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_settings.Shortcode}{_settings.PassKey}{timestamp}"));

            var payload = new
            {
                BusinessShortCode = _settings.Shortcode,
                Password = password,
                Timestamp = timestamp,
                TransactionType = "CustomerPayBillOnline",
                Amount = amount,
                PartyA = phoneNumber,
                PartyB = _settings.Shortcode,
                PhoneNumber = phoneNumber,
                CallBackURL = _settings.CallbackUrl,
                AccountReference = accountReference,
                TransactionDesc = transactionDesc
            };

            var jsonPayload = JsonConvert.SerializeObject(payload);

            var response = await client.PostAsync(_settings.LipaNaMpesaUrl, new StringContent(jsonPayload, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }
    public class MpesaB2BService
    {
        private readonly MpesaSettings _settings;
        private readonly MpesaAuthService _authService;

        public MpesaB2BService(IOptions<MpesaSettings> settings, MpesaAuthService authService)
        {
            _settings = settings.Value;
            _authService = authService;
        }

        public async Task<string> MakeBusinessPayment(string partyB, string amount, string accountReference, string requesterPhone, string remarks)
        {
            string accessToken = await _authService.GetAccessToken();

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var payload = new
            {
                Initiator = _settings.InitiatorName,  // Your API_Username
                SecurityCredentia = _settings.SecurityCredential, // Pre-encrypted initiator password
                CommandID = "BusinessPayBill",
                SenderIdentifierType = "4",  // Always 4
                RecieverIdentifierType = "4", // Always 4
                Amount = amount,
                PartyA = "600000", // Your Business Shortcode
                PartyB = partyB,               // Recipient Shortcode
                AccountReference = accountReference,
                Requester = requesterPhone,    // Optional: consumer phone number
                Remarks = remarks,
                QueueTimeOutURL = _settings.QueueTimeoutUrl,
                ResultURL = _settings.ResultUrl
            };

            var jsonPayload = JsonConvert.SerializeObject(payload);

            var response = await client.PostAsync(_settings.B2BUrl, new StringContent(jsonPayload, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }

}
