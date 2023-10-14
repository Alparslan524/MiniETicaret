using Application.Repositories.EntityRepository.InvoiceImageFileRepository;
using Domain.Entities.File;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.EntityRepository.InvoiceImageFileRepository
{
    public class InvoiceImageFileWriteRepository : WriteRepository<InvoiceImageFile>, IInvoiceImageFileWriteRepository
    {
        public InvoiceImageFileWriteRepository(EntityFrameworkDbContext context) : base(context)
        {
        }
    }
}
