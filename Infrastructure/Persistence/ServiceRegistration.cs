using Application.Abstractşons;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Concretes;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IProductService, ProductService>();

            serviceCollection.AddDbContext<EntityFrameworkDbContext>(options => options.UseSqlServer(@Configuration.ConnectionString));
            //Configuration.ConnectionString kullanılarak appsettings.jsondaki connections string çağırılıyor. 
            //Yani connections stringi el ile yazmaktansa daha dinamik hale getirdik.

        }
    }
}
