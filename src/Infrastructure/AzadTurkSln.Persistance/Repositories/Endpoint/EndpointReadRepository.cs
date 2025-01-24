using AzadTurkSln.Domain.Entities;
using AzadTurkSln.Persistance.Context;
using AzadTurkSln.Persistance.Repositories;
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
