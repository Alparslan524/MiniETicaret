using Application.Repositories.EntityRepository.ProductImageFileRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Commands.ProductImageFile.ChangeShowcaseImage
{
    public class ChangeShowcaseImageCommandHandler : IRequestHandler<ChangeShowcaseImageCommandRequest, ChangeShowcaseImageCommandResponse>
    {
        private IProductImageFileWriteRepository _productImageFileWriteRepository;

        public ChangeShowcaseImageCommandHandler(IProductImageFileWriteRepository productImageFileWriteRepository)
        {
            _productImageFileWriteRepository = productImageFileWriteRepository;
        }

        public async Task<ChangeShowcaseImageCommandResponse> Handle(ChangeShowcaseImageCommandRequest request, CancellationToken cancellationToken)
        {
            var query = _productImageFileWriteRepository.Table.Include(p => p.Product).SelectMany(p => p.Product, (pif, p) => new
            {
                pif,
                p
            });

            var data = await query.FirstOrDefaultAsync(p => p.p.Id == request.ProductId && p.pif.Showcase);

            if(data!=null)
                data.pif.Showcase = false;

            var image = await query.FirstOrDefaultAsync(p => p.pif.Id == request.ImageId);
            
            if(image!=null)
            image.pif.Showcase = true;

            await _productImageFileWriteRepository.SaveAsync();

            return new();
        }
    }
}
