using Domain.Entities;
using Domain.Entities.Common;
using Domain.Entities.File;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Contexts
{
    public class EntityFrameworkDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public EntityFrameworkDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Domain.Entities.File.File> Files { get; set; }
        public DbSet<InvoiceImageFile> InvoiceImageFiles { get; set; }
        public DbSet<ProductImageFile> ProductImageFiles { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Endpoint> Endpoints { get; set; }



        //SaveChangeAsync Interceptor. Gelen isteğin arasına girip override ile buradaki kodları çalıştıracak ve yoluna öyle devam edecek
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //ChangeTracker => Entityler üzerinde yapılan değişikliklerin ya da yeni eklenen verilerin yakalanmasını sağlayan property
            //Track edilen veriyi yakalayıp elde etmemizi sağlar(Track kutusu aklına gelsin.)

            //SaveChangesAsync metodu çalıştığında önce bu kodlar çalışacak. ChangeTracker ile değişiklik yapılacak veriyi/verileri yakaladık.
            //Eğer değişiklik add işlemiyse createDate ekledik
            //Eğer değişiklik update işlemiyse updateDate ekledik
            var datas = ChangeTracker.Entries<BaseEntity>();

            foreach (var data in datas)
            {
                if (data.State == EntityState.Added)
                {
                    data.Entity.CreateDate = DateTime.Now;
                }
                else
                    if (data.State == EntityState.Modified)
                {
                    data.Entity.UpdatedDate = DateTime.Now;
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
