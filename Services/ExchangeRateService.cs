using Newtonsoft.Json;

namespace Grocero.Services
{
    public class ExchangeRateService
    {
        private const string ApiBase = "https://v6.exchangerate-api.com/v6/";
        private const string ApiKey = "539263bf0727c0ce9c6de055"; // Api Key
        private readonly HttpClient _httpClient;

        public ExchangeRateService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<double> GetInrExchangeRateAsync()
        {
            var url = $"{ApiBase}{ApiKey}/latest/CAD";
            var response = await _httpClient.GetStringAsync(url);

            // Deserialize the response to get the INR exchange rate
            var exchangeRateApiResponse = JsonConvert.DeserializeObject<ExchangeRateApiResponse>(response);
            return exchangeRateApiResponse.ConversionRates.INR;
        }
    }

    public class ExchangeRateApiResponse
    {
        [JsonProperty("base_code")]
        public string Base { get; set; }

        [JsonProperty("conversion_rates")]
        public ConversionRates ConversionRates { get; set; }
    }

    public class ConversionRates
    {
        [JsonProperty("INR")]
        public double INR { get; set; }
    }
}
