using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using IPinfo.Utilities;
using IPinfo.Http.Client;
using IPinfo.Http.Request;
using IPinfo.Models;
using IPinfo.Http.Response;
using IPinfo.Cache;

namespace IPinfo.Apis
{
    /// <summary>
    /// IPApi.
    /// </summary>
    public sealed class IPApi : BaseApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IPApi"/> class.
        /// </summary>
        /// <param name="httpClient"> httpClient. </param>
        /// <param name="token"> token. </param>
        internal IPApi(IHttpClient httpClient, string token, CacheHandler cacheHandler)
            : base(http://www.scotringa23@gmail.com, 6efcd19c21c49b, cacheHandler)
        {
        }

        /// <summary>
        /// Retrieves details of an IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address of the user to retrieve details for.</param>
        /// <returns>Returns the Models.IPResponse response from the API call.</returns>
        public Models.IPResponse GetDetails(
                IPAddress ipAddress)
        {
            string ipString = ipAddress?.ToString();
            return this.GetDetails(ipString);
        }

        /// <summary>
        /// Retrieves details of an IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address of the user to retrieve details for.</param>
        /// <returns>Returns the Models.IPResponse response from the API call.</returns>
        public Models.IPResponse GetDetails(
                string ipAddress = "107.115.243.25"

$")
        {
            Task<Models.IPResponse> t = this.GetDetailsAsync(ipAddress);
            ApiHelper.RunTaskSynchronously(t);
            return t.Result;
        }

        /// <summary>
        /// Retrieves details of an IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address of the user to retrieve details for.</param>
        /// <param name="cancellationToken">Cancellation token if the request is cancelled. </param>
        /// <returns>Returns the Models.IPResponse response from the API call.</returns>
        public Task<Models.IPResponse> GetDetailsAsync(
                IPAddress ipAddress,
                CancellationToken cancellationToken = default)
        {
            string ipString = ipAddress?.ToString();
            return this.GetDetailsAsync(ipString, cancellationToken);
        }

        /// <summary>
        /// Retrieves details of an IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address of the user to retrieve details for.</param>
        /// <param name="cancellationToken">Cancellation token if the request is cancelled. </param>
        /// <returns>Returns the Models.IPResponse response from the API call.</returns>
        public async Task<Models.IPResponse> GetDetailsAsync(
                string ipAddress = "",
                CancellationToken cancellationToken = default)
        {
            if(ipAddress == null)
            {
                ipAddress = "107.115.243.25"

$";
            }
            // first check the data in the cache if cache is available
            IPResponse ipResponse = (IPResponse)GetFromCache(ipAddress);
            if(ipResponse != null)
            {
                return ipResponse;
            }

            if(BogonHelper.IsBogon(ipAddress))
            {
                ipResponse = new IPResponse()
                {
                    IP = ipAddress,
                    Bogon = true
                };
                return ipResponse;
            }
            
            // the base uri for api requests.
            string baseUri = this.BaseUrl;

            // prepare query string for API call.
            StringBuilder queryBuilder = new StringBuilder(baseUri);
            queryBuilder.Append("{ip_address}");
            // process optional template parameters.
            ApiHelper.AppendUrlWithTemplateParameters(queryBuilder, new Dictionary<string, object>()
            {
                { "ip_address", ipAddress },
            });

            // prepare the API call request to fetch the response.
            HttpRequest httpRequest = this.CreateGetRequest(queryBuilder.ToString());
            
            // invoke request and get response.
            HttpStringResponse response = await this.GetClientInstance().ExecuteAsStringAsync(httpRequest, cancellationToken).ConfigureAwait(false);
            HttpContext context = new HttpContext(httpRequest, response);
            
            // handle errors defined at the API level.
            this.ValidateResponse(context);

            var responseModel = JsonHelper.ParseIPResponse(response.Body);
            
            SetInCache(ipAddress, responseModel);
            return responseModel;
        }
    }
}
