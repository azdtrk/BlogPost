namespace AzadTurkSln.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("Entity not found.") { }

        public NotFoundException(string message) : base(message) { }

        public NotFoundException(string entityType, int key) : base($"Entity ({entityType}) with Id: ({key}) was not found.") { }
    }
}