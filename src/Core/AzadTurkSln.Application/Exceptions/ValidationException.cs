namespace AzadTurkSln.Application.Exceptions
{
    public class ValidationException : ApiException
    {
        public ValidationException() : base("Validation failure has occurred.") { }
        public ValidationException(string message) : base(message) { }
    }
} 