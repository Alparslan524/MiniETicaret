using Application.Repositories.EntityRepository.ProductRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        readonly private IProductReadRepository _productReadRepository;
        readonly private IProductWriteRepository _productWriteRepository;

        public ProductsController(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
        }

        [HttpGet]
        public async void Get()
        {
            await _productWriteRepository.AddRangeAsync(new()
            {
                new() {  Name="Product 1", Price = 100 , CreateDate = DateTime.Now, Stock = 1},
                new() {  Name="Product 2", Price = 10 , CreateDate = DateTime.Now, Stock = 10},
                new() {  Name="Product 3", Price = 130 , CreateDate = DateTime.Now, Stock = 132},
                new() {  Name="Product 4", Price = 5660 , CreateDate = DateTime.Now, Stock = 4}
            });
            await _productWriteRepository.SaveAsync();
        }//Deneme Nesneleri
    }
}
