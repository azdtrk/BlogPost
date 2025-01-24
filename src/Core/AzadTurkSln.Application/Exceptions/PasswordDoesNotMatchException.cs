namespace AzadTurkSln.Application.Exceptions
{
    public class PasswordDoesNotMatchException : ApiException
    {
        public PasswordDoesNotMatchException() : base("Password does not match!") { }
        public PasswordDoesNotMatchException(string message) : base(message) { }
    }
} 