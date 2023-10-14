using Application.Repositories.EntityRepository.FileRepository;
using Domain.Entities.File;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File = Domain.Entities.File.File;

namespace Persistence.Repositories.EntityRepository.FileRepository
{
    public class FileReadRepository : ReadRepository<File>, IFileReadRepository
    {
        public FileReadRepository(EntityFrameworkDbContext context) : base(context)
        {
        }
    }
}
