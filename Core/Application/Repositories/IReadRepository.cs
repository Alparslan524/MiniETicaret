using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IReadRepository<T> : IRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAll(bool tracking = true);
        IQueryable<T> GetWhere(Expression<Func<T, bool>>method, bool tracking = true);//Şarta uygun olan birden fazla veri
        Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true);////Şarta uygun olan tek veri
        Task<T> GetByIdAsync(int id, bool tracking = true);

    }
}
