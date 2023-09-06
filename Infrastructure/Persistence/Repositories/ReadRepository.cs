using Application.Repositories;
using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
    {
        private readonly EntityFrameworkDbContext _context;

        public ReadRepository(EntityFrameworkDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>(); //Buradaki Tableyi tüm tablo olarak düşünebiliriz

        public IQueryable<T> GetAll(bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)//tracking false ise AsNoTracking ile takip yapma
                query = query.AsNoTracking();
            return query;
        }

        //public IQueryable<T> GetAll()
        // => Table
        //Üstteki ile bu aynı şey

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true)
        {
            var query = Table.Where(method);
            if (!tracking)
                query = query.AsNoTracking();
            return query;
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();
            return await query.FirstOrDefaultAsync(method);
        }

        public async Task<T> GetByIdAsync(int id, bool tracking = true)
        {
            //return await Table.FirstOrDefaultAsync(data => data.Id == id);
            //return await Table.FindAsync(id);
            var query = Table.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();
            return await query.FirstOrDefaultAsync(data => data.Id == id);
        }
    }
}
