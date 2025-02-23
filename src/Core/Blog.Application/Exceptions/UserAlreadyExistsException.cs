namespace Blog.Application.Exceptions
{
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException() : base("User already exists.") { }

        public UserAlreadyExistsException(string userNameOrEmail) : base($"User ({userNameOrEmail}) already exists!") { }
    }
}