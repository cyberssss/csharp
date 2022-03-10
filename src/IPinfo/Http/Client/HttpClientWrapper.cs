using System.Net.Http;
using System.Net.Http.Headers;

using IPinfo.Models;
using IPinfo.Utilities;

namespace IPinfo.Http.Client
{
    
    public class HttpClientWrapper
    {

        private HttpClient client;

        public HttpClientWrapper(HttpClient httpClient)
        {
            this.client = httpClient;
        }

        public async Task<IPResponse> sendRequest(string token, string ip)
        {
            //TODO: just making plain request, need to change it

            try	
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await client.GetAsync($"http://ipinfo.io/{ip}");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                
                Console.WriteLine(responseBody);

                IPResponse ipResponse = JsonHelper.Deserialize<IPResponse>(responseBody);
                return ipResponse;
            }
            catch(HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");	
                Console.WriteLine("Message :{0} ",e.Message);
            }
            return null;   
        }
    }
}