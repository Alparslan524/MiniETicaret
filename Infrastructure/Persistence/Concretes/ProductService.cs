using Application.Abstractşons;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Concretes
{
    public class ProductService : IProductService
    {
        
        public List<Product> GetProducts()
            => new()
            {
                new() { Id=1,Name="Product 1", Price=100,Stock=10,CreateDate=DateTime.Now},
                new() { Id=2,Name="Product 2", Price=200,Stock=20,CreateDate=DateTime.Now},
                new() { Id=3,Name="Product 3", Price=300,Stock=30,CreateDate=DateTime.Now},
                new() { Id=4,Name="Product 4", Price=400,Stock=40,CreateDate=DateTime.Now},
                new() { Id=5,Name="Product 5", Price=500,Stock=50,CreateDate=DateTime.Now}
            };
        //{
        //    List<Product> liste = new List<Product>();
        //    Product product = new Product { Id = Guid.NewGuid(), Name = "product 1", Price = 100, Stock = 10 };
        //    liste.Add(product);
        //    return liste;
        //    Üsttekiyle bu aynı. Üstteki daha iyi.
        //}
    }
}
