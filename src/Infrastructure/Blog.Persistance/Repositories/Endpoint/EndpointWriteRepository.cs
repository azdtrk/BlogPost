using Blog.Domain.Entities;
using Blog.Persistance.Context;
using Blog.Persistance.Repositories;
using ETicaretAPI.Application.Repositories;

namespace ETicaretAPI.Persistence.Repositories
{
    public class EndpointWriteRepository : WriteRepository<Endpoint>, IEndpointWriteRepository
    {
        public EndpointWriteRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
