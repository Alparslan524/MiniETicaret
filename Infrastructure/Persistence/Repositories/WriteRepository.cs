using Application.Repositories;
using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class WriteRepository<T> : IWriteRepository<T> where T : BaseEntity
    {
        private readonly EntityFrameworkDbContext _context;

        public WriteRepository(EntityFrameworkDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();

        public async Task<bool> AddAsync(T model)
        {
            EntityEntry<T> entityEntry = await Table.AddAsync(model);
            return entityEntry.State == EntityState.Added;
            //İlk satırda ekleme işlemi yapılıyor ve sonuç bu eklenen nesnenin temsilcisine  (entityEntry) atanıyor
            //Alt satırda ise bu temsilci üzerinden durumu kontrol ediliyor. Eğer ekleme işlemi ise true değilse false dönüyor
        }

        public async Task<bool> AddRangeAsync(List<T> datas)
        {
            await Table.AddRangeAsync(datas);
            return true;//Burda nesneni temsilcisine erişemiyoruz haliyle durumuna bakamıyoruz.
        }

        public bool Remove(T model)
        {
            EntityEntry<T> entityEntry = Table.Remove(model);
            return entityEntry.State == EntityState.Deleted;//Temsilcinin durumu delete ise true dönüyor.
        }
        public bool RemoveRange(List<T> datas)
        {
            Table.RemoveRange(datas);
            return true;
        }
        public async Task<bool> RemoveAsync(int id)
        {
           T model = await Table.FirstOrDefaultAsync(data => data.Id == id);//Silinecek datayı bulduk
           return Remove(model);//Üstteki remove
           
            //Table.Remove(model);
           //return true; bu şekilde de diyebilirdik
        }

        public bool Update(T model)
        {
            EntityEntry<T> entityEntry = Table.Update(model);
            return entityEntry.State == EntityState.Modified;
        }
        public Task<int> SaveAsync()
        {
            return _context.SaveChangesAsync();
        }

        //Neden Task var? Çünkü async metodlar geriye Task döndürmek zorunda
    }
}
