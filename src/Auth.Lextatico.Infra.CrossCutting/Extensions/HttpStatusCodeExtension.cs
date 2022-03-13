using System.Net;

namespace Auth.Lextatico.Infra.CrossCutting.Extensions
{
    public static class HttpStatusCodeExtension
    {
        public static bool IsSuccess(this HttpStatusCode httpStatusCode)
        {
            var statusCode = (int) httpStatusCode;

            return statusCode >= 200 && statusCode <= 299;
        }

        public static bool IsSuccess(int statusCode) => statusCode >= 200 && statusCode <= 299;
    }
}
