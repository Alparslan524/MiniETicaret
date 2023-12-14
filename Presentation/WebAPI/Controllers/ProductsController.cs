using Application.Abstractions.Storage;
using Application.Consts;
using Application.CustomAttributes;
using Application.Enums;
using Application.Features.Commands.Product.CreateProduct;
using Application.Features.Commands.Product.DeleteProduct;
using Application.Features.Commands.Product.UpdateProduct;
using Application.Features.Commands.ProductImageFile.ChangeShowcaseImage;
using Application.Features.Commands.ProductImageFile.DeleteProductImage;
using Application.Features.Commands.ProductImageFile.UploadProductImage;
using Application.Features.Queries.Product.GetAllProduct;
using Application.Features.Queries.Product.GetByIdProduct;
using Application.Features.Queries.ProductImageFile.GetProductImageFile;
using Application.Repositories.EntityRepository.FileRepository;
using Application.Repositories.EntityRepository.InvoiceImageFileRepository;
using Application.Repositories.EntityRepository.ProductImageFileRepository;
using Application.Repositories.EntityRepository.ProductRepository;
using Application.RequestParameters;
using Application.ViewModels.Products;
using Domain.Entities;
using Domain.Entities.File;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        readonly IMediator _mediator;

        public ProductsController(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository, IWebHostEnvironment webHostEnvironment, IFileReadRepository fileReadRepository, IFileWriteRepository fileWriteRepository, IInvoiceImageFileReadRepository ınvoiceImageFileReadRepository, IInvoiceImageFileWriteRepository ınvoiceImageFileWriteRepository, IProductImageFileReadRepository productImageFileReadRepository, IProductImageFileWriteRepository productImageFileWriteRepository, IStorageService storageService, IConfiguration configuration, IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllProductQueryRequest request)
        {
            //GetAllProductQueryHandler : IRequestHandler<GetAllProductQueryRequest, GetAllProductQueryResponse> dediğimiz için 
            //getAllProductQueryRequest'in cevabı GetAllProductQueryResponse olduğunu biliyor.
            GetAllProductQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("{id}")]//Burdaki id requestteki ile aynı olması lazım. [FromRoute] dememizin sebebi .../api/products/getbyid/123 dicez
        public async Task<IActionResult> GetById([FromRoute] GetByIdProductQueryRequest request)
        {
            GetByIdProductQueryResponse response = await _mediator.Send(request);
            return Ok(response);
            //Product product = await _productReadRepository.GetByIdAsync(id);
            //return Ok(product);
            //İkisi aynı şey. İlkinde direkt herhangi bire atama yapmadan gönderdik
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Writing, Definition = "Create Product")]
        public async Task<IActionResult> Post(CreateProductCommandRequest request)
        {
            CreateProductCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Updating, Definition = "Update Product")]
        public async Task<IActionResult> Put([FromBody] UpdateProductCommandRequest request)
        {
            UpdateProductCommandResponse response = await _mediator.Send(request);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Deleting, Definition = "Delete Product")]
        public async Task<IActionResult> Delete([FromRoute] DeleteProductCommandRequest request)
        {
            DeleteProductCommandResponse response = await _mediator.Send(request);
            return Ok();
        }

        [HttpPost("[action]")]//products?id=8 gibi(action dediğimiz için clientte actionu bildirmemiz lazım)
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Writing, Definition = "Upload Product File")]
        public async Task<IActionResult> Upload([FromQuery, FromBody] UploadProductImageCommandRequest request)
        {
            request.Files = Request.Form.Files;//?? anlamadım. Ama galiba şey => Queryden gelen files'i requesttekine atadık.
            UploadProductImageCommandResponse response = await _mediator.Send(request);
            return Ok();
        }

        [HttpGet("[action]/{id}")]//products/8 gibi
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Reading, Definition = "Get Product Image")]
        public async Task<IActionResult> GetProductImages([FromRoute] GetProductImageQueryRequest request)
        {
            List<GetProductImageQueryResponse> response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpDelete("[action]/{id}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Deleting, Definition = "Delete Product Image")]
        public async Task<IActionResult> DeleteProductImage([FromRoute] DeleteProductImageCommandRequest request, [FromQuery] int imageId)
        {
            request.imageId = imageId;
            DeleteProductImageCommandResponse response = await _mediator.Send(request);
            return Ok();
        }

        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Updating, Definition = "Change Showcase Image")]
        public async Task<IActionResult> ChangeShowcaseImage([FromQuery] ChangeShowcaseImageCommandRequest request)
        {
            ChangeShowcaseImageCommandResponse response = await _mediator.Send(request);
            return Ok(response);
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