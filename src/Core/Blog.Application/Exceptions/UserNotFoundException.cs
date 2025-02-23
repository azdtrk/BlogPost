namespace Blog.Application.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException() : base("User not found.") { }

        public UserNotFoundException(string email) : base($"User with email: ({email}) was not found.") { }
        
        public UserNotFoundException(Guid UserId) : base($"User with Id: ({UserId}) was not found.") { }


    }
}