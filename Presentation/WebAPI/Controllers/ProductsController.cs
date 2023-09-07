using Application.Repositories.EntityRepository.ProductRepository;
using Domain.Entities;
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
        public async Task Get()
        {
            await _productWriteRepository.AddRangeAsync(new()
                {
                    new() {  Name="Product 11", Price = 100.0 , Stock = 1},
                    new() {  Name="Product 12", Price = 10.0 , Stock = 10},
                    new() {  Name="Product 13", Price = 130.0 , Stock = 132},
                    new() {  Name="Product 14", Price = 5660.0 , Stock = 4}
                });//Deneme Nesneleri
            await _productWriteRepository.SaveAsync();


            //Tracking denemesi
            //Tracking değeri true ise herhangi bişey değişmiyor fakat eğer false ise bu sefer fiziksel veritabanına
            //değişikliği yapmıyor. Yani false ile Tracking havuzuna bu değişikliği koymuyoruz ve bu yüzden 
            //SaveAsync() metodu bu değişikliği göremiyor ve db'ye bu değişiklik yansımıyor.

            //Product product = await _productReadRepository.GetByIdAsync(2,false);
            //product.Name = "Tracking!!!!!!";
            //await _productWriteRepository.SaveAsync();
        }




        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _productReadRepository.GetByIdAsync(id));
            //Product product = await _productReadRepository.GetByIdAsync(id);
            //return Ok(product);
            //İkisi aynı şey. İlkinde direkt herhangi bire atama yapmadan gönderdik
        }

    }
}
