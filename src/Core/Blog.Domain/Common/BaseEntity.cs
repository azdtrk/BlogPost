namespace Blog.Domain.Common
{
    public class BaseEntity 
    {
        public Guid Id { get; set; }

        public DateTime DateCreated { get; set; }

        public BaseEntity()
        {
            DateCreated = DateTime.UtcNow;
        }
    }
}
