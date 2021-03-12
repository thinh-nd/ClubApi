using System.Net;

namespace ClubApi.Exceptions
{
    public class EntityExistedException : ClientException
    {
        public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;

        public override string Message => $"{_property} already has value {_value}.";

        private readonly string _property;
        private readonly string _value;

        public EntityExistedException(string property, string value)
        {
            _property = property;
            _value = value;
        }
    }
}
