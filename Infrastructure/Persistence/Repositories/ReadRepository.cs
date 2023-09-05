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

        public IQueryable<T> GetAll()
        {
            return Table;
        }

        //public IQueryable<T> GetAll()
        // => Table
        //Üstteki ile bu aynı şey

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> method)
        {
            return Table.Where(method);
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> method)
        {
            return await Table.FirstOrDefaultAsync(method);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            //return await Table.FirstOrDefaultAsync(data => data.Id == id);
            return await Table.FindAsync(id);
        }
    }
}
