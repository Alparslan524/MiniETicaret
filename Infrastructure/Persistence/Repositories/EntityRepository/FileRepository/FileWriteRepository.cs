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
    public class FileWriteRepository : WriteRepository<File> , IFileWriteRepository
    {
        public FileWriteRepository(EntityFrameworkDbContext context) : base(context)
        {
        }
    }
}
