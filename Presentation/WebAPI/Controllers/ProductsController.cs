using Application.Repositories.EntityRepository.ProductRepository;
using Application.RequestParameters;
using Application.ViewModels.Products;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        public async Task<IActionResult> Get([FromQuery]Pagination pagination)
        {
            //Kullanıcıya sadece veri gönderdiğimiz için bunların Tracking havuzuna girmesine gerek yok ve bu yüzden track değeri false
            //Kullanıcıya veri sunacağımız zaman veritabanında herhangi bir değişiklik yapmayacağımız için false olarak kullanabiliriz.

            var totalCount = _productReadRepository.GetAll(false).Count();
            var products = _productReadRepository.GetAll(false).Select(p => new
            {//Cliente sadece bu verileri göndericez.
                p.Id,
                p.Name,
                p.Stock,
                p.Price,
                p.CreateDate,
                p.UpdatedDate
            }).Skip(pagination.Page * pagination.Size).Take(pagination.Size);
            //page 3 size 10 olsun. 30 tanesini atla 10 tane getir. Yani sayfalama işlemi
            return Ok(new
            {
                totalCount, 
                products
            });
            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _productReadRepository.GetByIdAsync(id, false));
            //Product product = await _productReadRepository.GetByIdAsync(id);
            //return Ok(product);
            //İkisi aynı şey. İlkinde direkt herhangi bire atama yapmadan gönderdik
        }

        [HttpPost]
        public async Task<IActionResult> Post(VM_Create_Product model)
        {
            await _productWriteRepository.AddAsync(new()
            {
                Name = model.Name,
                Price = model.Price,
                Stock = model.Stock
            });
            await _productWriteRepository.SaveAsync();
            return Ok((int)HttpStatusCode.Created);
            //HttpStatusCode enumu üzerinden created kodunu inte çevirip gönderiyor
        }

        [HttpPut]
        public async Task<IActionResult> Put(VM_Update_Product model)
        {
            Product product = await _productReadRepository.GetByIdAsync(model.Id);
            product.Name = model.Name;
            product.Price = model.Price;
            product.Stock = model.Stock;
            await _productWriteRepository.SaveAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productWriteRepository.RemoveAsync(id);
            await _productWriteRepository.SaveAsync();
            return Ok();
        }
    }
}




//Tracking denemesi
//Tracking değeri true ise herhangi bişey değişmiyor fakat eğer false ise bu sefer fiziksel veritabanına
//değişikliği yapmıyor. Yani false ile Tracking havuzuna bu değişikliği koymuyoruz ve bu yüzden 
//SaveAsync() metodu bu değişikliği göremiyor ve db'ye bu değişiklik yansımıyor.
//Kullanıcıya veri sunacağımız zaman veritabanında herhangi bir değişiklik yapmayacağımız için false olarak kullanabiliriz.

//Product product = await _productReadRepository.GetByIdAsync(2,false);
//product.Name = "Tracking!!!!!!";
//await _productWriteRepository.SaveAsync();