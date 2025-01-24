using AzadTurkSln.Domain.Entities;
using AzadTurkSln.Persistance.Context;
using AzadTurkSln.Persistance.Repositories;
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
