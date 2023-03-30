using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EnergyInforamtionBot
{
    /// <summary>
    /// Client that calls EIA to collect data
    /// </summary>
    public class EIAClient
    {

        /// <summary>
        /// Creates an EIA Client
        /// </summary>
        /// <param name="configuration">Injected by host</param>
        public EIAClient(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new HttpClient();
            _uri = new Uri(string.Format(configuration["restUrl"], string.Empty));
        }

        /// <summary>
        /// Returns the series data as a list of EIASeriesItems
        /// </summary>
        /// <returns>List of EIASeriesItems</returns>
        public async Task<List<EIASeriesItem>> GetSeries()
        {
            HttpResponseMessage response = await _client.GetAsync(_uri);
            if (response.IsSuccessStatusCode)
            {
                List<EIASeriesItem> result = new List<EIASeriesItem>();
                string data = await response.Content.ReadAsStringAsync();
                var series = JsonConvert.DeserializeObject<JObject>(data);

                var json = series["response"]["data"];
                var count = json.Count();
                for (var i = 0; i < count; i++)
                {
                    var item = json[i];
                    var period = item["period"].ToString();
                    var price = item["value"].ToString();
                    result.Add(new EIASeriesItem { Period = DateTime.Parse(period), Price = float.Parse(price) });
                }

                return result;
            }
            return null;
        }


        private HttpClient _client;
        private Uri _uri;
        private readonly IConfiguration _configuration;
    }
}
