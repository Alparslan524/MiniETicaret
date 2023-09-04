using Application.Repositories;
using Application.Repositories.Entity_Repository.CustomerRepository;
using Application.Repositories.EntityRepository.CustomerRepository;
using Application.Repositories.EntityRepository.OrderRepository;
using Application.Repositories.EntityRepository.ProductRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using Persistence.Repositories.EntityRepository.CustomerRepository;
using Persistence.Repositories.EntityRepository.OrderRepository;
using Persistence.Repositories.EntityRepository.ProductRepository;
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
            //DB bağlantısı
            serviceCollection.AddDbContext<EntityFrameworkDbContext>(options => options.UseSqlServer(@Configuration.ConnectionString), ServiceLifetime.Singleton);
            //Configuration.ConnectionString kullanılarak appsettings.jsondaki connections string çağırılıyor. 
            //Yani connections stringi el ile yazmaktansa daha dinamik hale getirdik.


            serviceCollection.AddSingleton<ICustomerReadRepository, CustomerReadRepository>();
            serviceCollection.AddSingleton<ICustomerWriteRepository, CustomerWriteRepository>();
            
            serviceCollection.AddSingleton<IOrderReadRepository, OrderReadRepository>();
            serviceCollection.AddSingleton<IOrderWriteRepository,OrderWriteRepository>();

            serviceCollection.AddSingleton<IProductReadRepository,ProductReadRepository>();
            serviceCollection.AddSingleton<IProductWriteRepository,ProductWriteRepository>();



            

        }
    }
}
