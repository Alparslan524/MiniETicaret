using Application.Abstractions.Storage;
using Application.Features.Commands.CreateProduct;
using Application.Features.Queries.GetAllProduct;
using Application.Repositories.EntityRepository.FileRepository;
using Application.Repositories.EntityRepository.InvoiceImageFileRepository;
using Application.Repositories.EntityRepository.ProductImageFileRepository;
using Application.Repositories.EntityRepository.ProductRepository;
using Application.RequestParameters;
using Application.ViewModels.Products;
using Domain.Entities;
using Domain.Entities.File;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        readonly private IProductReadRepository _productReadRepository;
        readonly private IProductWriteRepository _productWriteRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        readonly IFileReadRepository _fileReadRepository;
        readonly IFileWriteRepository _fileWriteRepository;

        readonly IInvoiceImageFileReadRepository _ınvoiceImageFileReadRepository;
        readonly IInvoiceImageFileWriteRepository _ınvoiceImageFileWriteRepository;

        readonly IProductImageFileReadRepository _productImageFileReadRepository;
        readonly IProductImageFileWriteRepository _productImageFileWriteRepository;

        readonly IStorageService _storageService;

        readonly IConfiguration _configuration;

        readonly IMediator _mediator;




        public ProductsController(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository, IWebHostEnvironment webHostEnvironment, IFileReadRepository fileReadRepository, IFileWriteRepository fileWriteRepository, IInvoiceImageFileReadRepository ınvoiceImageFileReadRepository, IInvoiceImageFileWriteRepository ınvoiceImageFileWriteRepository, IProductImageFileReadRepository productImageFileReadRepository, IProductImageFileWriteRepository productImageFileWriteRepository, IStorageService storageService, IConfiguration configuration, IMediator mediator)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _webHostEnvironment = webHostEnvironment;
            _fileReadRepository = fileReadRepository;
            _fileWriteRepository = fileWriteRepository;
            _ınvoiceImageFileReadRepository = ınvoiceImageFileReadRepository;
            _ınvoiceImageFileWriteRepository = ınvoiceImageFileWriteRepository;
            _productImageFileReadRepository = productImageFileReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _storageService = storageService;
            _configuration = configuration;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllProductQueryRequest getAllProductQueryRequest)
        {
            //GetAllProductQueryHandler : IRequestHandler<GetAllProductQueryRequest, GetAllProductQueryResponse> dediğimiz için 
            //getAllProductQueryRequest'in cevabı GetAllProductQueryResponse olduğunu biliyor.
            GetAllProductQueryResponse response = await _mediator.Send(getAllProductQueryRequest);
            return Ok(response);
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
        public async Task<IActionResult> Post(CreateProductCommandRequest createProductCommandRequest)
        {
            CreateProductCommandResponse response = await _mediator.Send(createProductCommandRequest);
            return Ok(response);
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

        [HttpPost("[action]")]//products?id=8 gibi(action dediğimiz için clientte actionu bildirmemiz lazım)
        public async Task<IActionResult> Upload(int id)
        {

            List<(string fileName, string pathOrContainerName)> results =
                await _storageService.UploadAsync("photo-images", Request.Form.Files);

            Product product = await _productReadRepository.GetByIdAsync(id);

            await _productImageFileWriteRepository.AddRangeAsync(results.Select(r => new ProductImageFile
            {
                FileName = r.fileName,
                Path = r.pathOrContainerName,
                Storage = _storageService.StorageName,
                Product = new List<Product>() { product }
            }).ToList());

            await _productImageFileWriteRepository.SaveAsync();

            return Ok();


            //var datas = await _storageService.UploadAsync("files", Request.Form.Files);

            //await _productImageFileWriteRepository.AddRangeAsync(datas.Select(d => new ProductImageFile()
            //{
            //    FileName = d.fileName,
            //    Path = d.pathOrContainerName,
            //    Storage = _storageService.StorageName
            //}).ToList());
            //await _productImageFileWriteRepository.SaveAsync();

            //return Ok();


            //wwwroot/resource/product-images
            //var datas = await _fileService.UploadAsync("resource/files", Request.Form.Files);

            //await _productImageFileWriteRepository.AddRangeAsync(datas.Select(d => new ProductImageFile()
            //{
            //    FileName = d.fileName,
            //    Path = d.path
            //}).ToList());
            //await _productImageFileWriteRepository.SaveAsync();

            //await _ınvoiceImageFileWriteRepository.AddRangeAsync(datas.Select(d => new InvoiceImageFile()
            //{
            //    FileName = d.fileName,
            //    Path = d.path,
            //    Price = 500
            //}).ToList());
            //await _ınvoiceImageFileWriteRepository.SaveAsync();

            //await _fileWriteRepository.AddRangeAsync(datas.Select(d => new Domain.Entities.File.File()
            //{
            //    FileName = d.fileName,
            //    Path = d.path
            //}).ToList());
            //await _fileWriteRepository.SaveAsync();
        }

        [HttpGet("[action]/{id}")]//products/8 gibi
        public async Task<IActionResult> GetProductImages(int id)
        {
            Product? product = await _productReadRepository.Table.Include(p => p.ProductImageFiles)
                .FirstOrDefaultAsync(p => p.Id == id);//Id'si 8 olan productın ProductImageFiles bilgilerini getirir.(action dediğimiz için clientte actionu bildirmemiz lazım)

            return Ok(product.ProductImageFiles.Select(p => new
            {
                Path = $"{_configuration["BaseStorageUrl"]}/{p.Path}",
                p.FileName,
                p.Id
            }));
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteProductImage(int id, int imageId)
        {
            Product? product = await _productReadRepository.Table.Include(p => p.ProductImageFiles)
                .FirstOrDefaultAsync(p => p.Id == id);

            ProductImageFile productImageFile = product.ProductImageFiles.FirstOrDefault(p => p.Id == imageId);

            product.ProductImageFiles.Remove(productImageFile);
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