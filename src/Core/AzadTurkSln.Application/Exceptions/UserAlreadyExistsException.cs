namespace AzadTurkSln.Application.Exceptions
{
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException() : base("User already exists found.") { }

        public UserAlreadyExistsException(string email) : base($"A user with email: ({email}) already exists!") { }
    }
}