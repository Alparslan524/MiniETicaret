using Application.Repositories.EntityRepository.ProductRepository;
using Application.RequestParameters;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Queries.GetAllProduct
{//Burada IRequestHandler ile Handler sınıfımıza request ve responseleri veriyoruz.
    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQueryRequest, GetAllProductQueryResponse>
    {
        readonly IProductReadRepository _productReadRepository;

        public GetAllProductQueryHandler(IProductReadRepository productReadRepository)
        {
            _productReadRepository = productReadRepository;
        }

        public async Task<GetAllProductQueryResponse> Handle(GetAllProductQueryRequest request, CancellationToken cancellationToken)
        {
            var totalCount = _productReadRepository.GetAll(false).Count();
            var products = _productReadRepository.GetAll(false).Skip(request.Page * request.Size).Take(request.Size)
                .Select(p => new
                {//Cliente sadece bu verileri göndericez.
                    p.Id,
                    p.Name,
                    p.Stock,
                    p.Price,
                    p.CreateDate,
                    p.UpdatedDate
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
