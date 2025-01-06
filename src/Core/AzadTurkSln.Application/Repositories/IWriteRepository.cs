using AzadTurkSln.Domain.Common;

namespace AzadTurkSln.Application.Repositories
{
    public interface IWriteRepository<T> : IRepository<T> where T : BaseEntity
    {
        Task<bool> AddAsync(T model);
        bool Remove(T model);
        Task<bool> RemoveAsync(int id);
        bool Update(T model);
    }
}
