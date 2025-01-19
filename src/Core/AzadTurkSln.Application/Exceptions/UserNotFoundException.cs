namespace AzadTurkSln.Application.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException() : base("User not found.") { }

        public UserNotFoundException(string email) : base($"User with email: ({email}) was not found.") { }
    }
}