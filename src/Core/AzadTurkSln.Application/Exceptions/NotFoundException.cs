namespace AzadTurkSln.Application.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException() : base("Entity not found.") { }

        public EntityNotFoundException(string message) : base(message) { }

        public EntityNotFoundException(string entityType, Guid key) : base($"Entity ({entityType}) with Id: ({key}) was not found.") { }
    }
}