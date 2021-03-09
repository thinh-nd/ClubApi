using System;
using System.Net;

namespace ClubApi.Exceptions
{
    public class ClientException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
    }
}
