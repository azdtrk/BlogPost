namespace AzadTurkSln.Application.Exceptions
{
    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException() : base("Please check your credentials!") { }

    }
}