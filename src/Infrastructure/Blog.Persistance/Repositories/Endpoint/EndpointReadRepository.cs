using Blog.Domain.Entities;
using Blog.Persistance.Context;
using Blog.Persistance.Repositories;
using ETicaretAPI.Application.Repositories;

namespace ETicaretAPI.Persistence.Repositories
{
    public class EndpointReadRepository : ReadRepository<Endpoint>, IEndpointReadRepository
    {
        public EndpointReadRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
