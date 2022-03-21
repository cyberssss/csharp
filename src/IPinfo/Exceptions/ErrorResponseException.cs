using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

using IPinfo.Models;
using IPinfo.Utilities;
using IPinfo.Http.Request;
using IPinfo.Http.Response;
using IPinfo.Http.Client;

namespace IPinfo.Exceptions
{
    
    /// <summary>
    /// This is the class for all exceptions that represent an error response
    /// from the server.
    /// </summary>
    public class ErrorResponseException : BaseApiException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResponseException"/> class.
        /// </summary>
        /// <param name="reason"> The reason for throwing exception.</param>
        /// <param name="context"> The HTTP context that encapsulates request and response objects.</param>
        public ErrorResponseException(HttpContext context)
            : base("", context)
        {
            
        }
    }
}