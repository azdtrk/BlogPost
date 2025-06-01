using Blog.Application.Repositories.Endpoint;
using Blog.Persistance.Context;

namespace Blog.Persistance.Repositories.Endpoint
{
    public class EndpointWriteRepository : WriteRepository<Domain.Entities.Endpoint>, IEndpointWriteRepository
    {
        public EndpointWriteRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
