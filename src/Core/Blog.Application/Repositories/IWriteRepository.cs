using Blog.Domain.Common;

namespace Blog.Application.Repositories
{
    public interface IWriteRepository<T> : IRepository<T> where T : BaseEntity
    {
        Task<bool> AddAsync(T model);
        bool Remove(T model);
        Task<bool> RemoveAsync(Guid id);
        bool Update(T model);
    }
}
