
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;


namespace Services.Layer
{
    public static class CallApi
    {
        public static async Task<string> Quotation(string currency)
        {

            var httpClient = HttpClientFactory.Create();
            var url = "http://api.cambio.today/v1/quotes/" + currency + "/ARS/json?key=4545|EHmKjxp8xh1xvo8oP~~2EM*ta9Ve4P_W";
            var data = await httpClient.GetStringAsync(url);

            var myDeserializedObj = JsonConvert.DeserializeObject(data);

            return data;
        }

    }
}
