using Domain.Entities.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File = Domain.Entities.File.File;

namespace Application.Repositories.EntityRepository.FileRepository
{
    public interface IFileWriteRepository : IWriteRepository<File>
    {
    }
}
