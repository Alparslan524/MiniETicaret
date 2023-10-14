using Application.Repositories.EntityRepository.ProductImageFileRepository;
using Domain.Entities.File;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.EntityRepository.ProductImageFileRepository
{
    public class ProductImageFileWriteRepository : WriteRepository<ProductImageFile>, IProductImageFileWriteRepository
    {
        public ProductImageFileWriteRepository(EntityFrameworkDbContext context) : base(context)
        {
        }
    }
}
