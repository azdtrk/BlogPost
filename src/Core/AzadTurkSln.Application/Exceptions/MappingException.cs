namespace AzadTurkSln.Application.Exceptions
{
    public class MappingException : Exception
    {
        public MappingException() : base("Mapping failure has occurred.") { }
        public MappingException(string message) : base(message) { }
    }
} 