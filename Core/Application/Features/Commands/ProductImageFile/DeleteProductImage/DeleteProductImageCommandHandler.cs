using Application.Repositories.EntityRepository.ProductRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using P = Domain.Entities.Product;
using PIF = Domain.Entities.File.ProductImageFile;

namespace Application.Features.Commands.ProductImageFile.DeleteProductImage
{
    public class DeleteProductImageCommandHandler : IRequestHandler<DeleteProductImageCommandRequest, DeleteProductImageCommandResponse>
    {
        readonly IProductReadRepository _productReadRepository;
        readonly IProductWriteRepository _productWriteRepository;

        public DeleteProductImageCommandHandler(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
        }

        public async Task<DeleteProductImageCommandResponse> Handle(DeleteProductImageCommandRequest request, CancellationToken cancellationToken)
        {
            P? product = await _productReadRepository.Table.Include(p => p.ProductImageFiles)
               .FirstOrDefaultAsync(p => p.Id == request.id);

            PIF? productImageFile = product?.ProductImageFiles.FirstOrDefault(p => p.Id == request.imageId);

            if (productImageFile != null)
            {
                product?.ProductImageFiles.Remove(productImageFile);
            }
            await _productWriteRepository.SaveAsync();
            
            return new();
        }
    }
}
