using Hospital.Business.Dtos;
using Hospital.Business.Services.Abstract;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hospital.Business.Services.Concrete
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly PaymentConfigurationDto _paymentConfigurationDto;

        public PaymentService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient();
            _paymentConfigurationDto = _configuration.GetSection("PaymentSettings").Get<PaymentConfigurationDto>() ?? new();
        }

        public async Task<string> CreateTestPaymentAsync(decimal amount, string description)
        {
            string requestBody = $@"
            {{
                ""order"": {{
                    ""typeRid"": ""Order_SMS"",
                    ""amount"": {amount.ToString().Replace(',', '.')},
                    ""currency"": ""AZN"",
                    ""language"": ""az"",
                    ""description"": ""{description}"",
                    ""hppRedirectUrl"": ""https://localhost:5001/payment/check"" 
                }}
            }}";

            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_paymentConfigurationDto.Username}:{_paymentConfigurationDto.Password}"));
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_paymentConfigurationDto.BaseUrl, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseContent); // Test üçün console log
            return responseContent;
        }

        public async Task<bool> CheckPaymentAsync(PaymentCheckDto dto)
        {
            // Burada sadəcə test məqsədli logic
            // Real implementasiyada Bank API çağırıb status yoxlanacaq
            return await Task.FromResult(dto.STATUS == Hospital.Business.Core.PaymentStatuses.FullyPaid);
        }
    }
}