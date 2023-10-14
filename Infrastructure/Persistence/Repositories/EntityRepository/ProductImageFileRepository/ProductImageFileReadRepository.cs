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
    public class ProductImageFileReadRepository : ReadRepository<ProductImageFile>, IProductImageFileReadRepository
    {
        public ProductImageFileReadRepository(EntityFrameworkDbContext context) : base(context)
        {
        }
    }
}
