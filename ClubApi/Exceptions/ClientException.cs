using System;
using System.Net;

namespace ClubApi.Exceptions
{
    public class ClientException : Exception
    {
        public virtual HttpStatusCode StatusCode { get; }
    }
}
