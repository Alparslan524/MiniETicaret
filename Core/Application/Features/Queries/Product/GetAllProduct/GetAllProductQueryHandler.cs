using Application.Repositories.EntityRepository.ProductRepository;
using Application.RequestParameters;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Queries.Product.GetAllProduct
{//Burada IRequestHandler ile Handler sınıfımıza request ve responseleri veriyoruz.
    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQueryRequest, GetAllProductQueryResponse>
    {
        readonly IProductReadRepository _productReadRepository;
        readonly ILogger<GetAllProductQueryHandler> _logger;

        public GetAllProductQueryHandler(IProductReadRepository productReadRepository, ILogger<GetAllProductQueryHandler> logger)
        {
            _productReadRepository = productReadRepository;
            _logger = logger;
        }

        public async Task<GetAllProductQueryResponse> Handle(GetAllProductQueryRequest request, CancellationToken cancellationToken)
        {
            //throw new Exception("Global Exception hatası denemesi");
            _logger.LogInformation("Bütün ürünler listelendi. Loglama denemesi");

            var totalCount = _productReadRepository.GetAll(false).Count();
            var products = _productReadRepository.GetAll(false).Skip(request.Page * request.Size).Take(request.Size)
                .Include(p => p.ProductImageFiles)
                .Select(p => new
                {//Cliente sadece bu verileri göndericez.
                    p.Id,
                    p.Name,
                    p.Stock,
                    p.Price,
                    p.CreateDate,
                    p.UpdatedDate,
                    p.ProductImageFiles
                }).ToList();
            //page 3 size 10 olsun. 30 tanesini atla 10 tane getir. Yani sayfalama işlemi
            return new()
            {
                Products = products,
                TotalCount = totalCount
            };
        }
    }
}
