using Blog.Application.Repositories.Endpoint;
using Blog.Persistance.Context;

namespace Blog.Persistance.Repositories.Endpoint
{
    public class EndpointReadRepository : ReadRepository<Domain.Entities.Endpoint>, IEndpointReadRepository
    {
        public EndpointReadRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
