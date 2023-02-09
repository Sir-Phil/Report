using System.Net;

namespace Articles.Infrastructure.Erros
{
    public class RestException : Exception
    {
        public RestException(HttpStatusCode code, object errors = null)
        {
            Code = code;
            Errors = errors;
        }

        public HttpStatusCode Code { get; set; }
        public object Errors { get; set; }
    }
}
