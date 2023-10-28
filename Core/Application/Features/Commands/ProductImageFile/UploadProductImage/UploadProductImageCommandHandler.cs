using Application.Abstractions.Storage;
using Application.Repositories.EntityRepository.ProductImageFileRepository;
using Application.Repositories.EntityRepository.ProductRepository;
using Domain.Entities.File;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using P = Domain.Entities.Product;
using PIF = Domain.Entities.File.ProductImageFile;

namespace Application.Features.Commands.ProductImageFile.UploadProductImage
{
    public class UploadProductImageCommandHandler : IRequestHandler<UploadProductImageCommandRequest, UploadProductImageCommandResponse>
    {
        readonly IStorageService _storageService;
        readonly IProductReadRepository _productReadRepository;
        readonly IProductImageFileWriteRepository _productImageFileWriteRepository;

        public UploadProductImageCommandHandler(IStorageService storageService, IProductImageFileWriteRepository productImageFileWriteRepository, IProductReadRepository productReadRepository)
        {
            _storageService = storageService;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _productReadRepository = productReadRepository;
        }

        public async Task<UploadProductImageCommandResponse> Handle(UploadProductImageCommandRequest request, CancellationToken cancellationToken)
        {

            List<(string fileName, string pathOrContainerName)> results =
            await _storageService.UploadAsync("photo-images", request.Files);

            P product = await _productReadRepository.GetByIdAsync(request.id);
            
            await _productImageFileWriteRepository.AddRangeAsync(results.Select(r => new PIF
            {
                FileName = r.fileName,
                Path = r.pathOrContainerName,
                Storage = _storageService.StorageName,
                Product = new List<P>() { product }
            }).ToList());

            await _productImageFileWriteRepository.SaveAsync();

            return new();
        }
    }
}
