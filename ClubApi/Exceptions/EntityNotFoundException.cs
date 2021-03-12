using System.Net;

namespace ClubApi.Exceptions
{
    public class EntityNotFoundException : ClientException
    {
        public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;

        public override string Message => $"{_property} does not has value {_value}.";

        private readonly string _property;
        private readonly string _value;

        public EntityNotFoundException(string property, string value)
        {
            _property = property;
            _value = value;
        }
    }
}
