using AzadTurkSln.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace AzadTurkSln.Application.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        DbSet<T> Table { get; }
    }
}
