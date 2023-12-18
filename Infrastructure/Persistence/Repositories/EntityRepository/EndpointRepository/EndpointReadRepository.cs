using Application.Repositories.EntityRepository.EndpointRepository;
using Domain.Entities;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.EntityRepository.EndpointRepository
{
    public class EndpointReadRepository : ReadRepository<Endpoint>, IEndpointReadRepository
    {
        public EndpointReadRepository(EntityFrameworkDbContext context) : base(context)
        {
        }
    }
}
