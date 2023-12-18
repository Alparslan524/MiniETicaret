using Application.Repositories.EntityRepository.MenuRepository;
using Domain.Entities;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.EntityRepository.MenuRepository
{
    public class MenuWriteRepository : WriteRepository<Menu>, IMenuWriteRepository
    {
        public MenuWriteRepository(EntityFrameworkDbContext context) : base(context)
        {
        }
    }
}
