using Application.Abstractşons;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Concretes;
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
        }
    }
}
