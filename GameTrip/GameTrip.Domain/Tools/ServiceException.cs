using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GameTrip.Domain.Tools
{
    public class ServiceException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public ServiceException(HttpStatusCode statusCode, Exception? innerException = null) : this(statusCode, string.Empty, innerException) { }
        public ServiceException(string message, Exception? innerException = null) : this(HttpStatusCode.BadRequest, message, innerException) { }
        public ServiceException(HttpStatusCode statusCode, string message, Exception? innerException = null) : base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }
}
