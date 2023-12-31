﻿using Application.Abstractions.Services;
using Application.Abstractions.Services.Authentication;
using Application.Repositories;
using Application.Repositories.Entity_Repository.CustomerRepository;
using Application.Repositories.EntityRepository.CustomerRepository;
using Application.Repositories.EntityRepository.EndpointRepository;
using Application.Repositories.EntityRepository.FileRepository;
using Application.Repositories.EntityRepository.InvoiceImageFileRepository;
using Application.Repositories.EntityRepository.MenuRepository;
using Application.Repositories.EntityRepository.OrderRepository;
using Application.Repositories.EntityRepository.ProductImageFileRepository;
using Application.Repositories.EntityRepository.ProductRepository;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using Persistence.Repositories.EntityRepository.CustomerRepository;
using Persistence.Repositories.EntityRepository.EndpointRepository;
using Persistence.Repositories.EntityRepository.FileRepository;
using Persistence.Repositories.EntityRepository.InvoiceImageFileRepository;
using Persistence.Repositories.EntityRepository.MenuRepository;
using Persistence.Repositories.EntityRepository.OrderRepository;
using Persistence.Repositories.EntityRepository.ProductImageFileRepository;
using Persistence.Repositories.EntityRepository.ProductRepository;
using Persistence.Services;
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
            serviceCollection.AddDbContext<EntityFrameworkDbContext>(options => options.UseSqlServer(@Configuration.ConnectionString));
            //Configuration.ConnectionString kullanılarak appsettings.jsondaki connections string çağırılıyor. 
            //Yani connections stringi el ile yazmaktansa daha dinamik hale getirdik.

            serviceCollection.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<EntityFrameworkDbContext>().
            AddDefaultTokenProviders();

            serviceCollection.AddScoped<ICustomerReadRepository, CustomerReadRepository>();
            serviceCollection.AddScoped<ICustomerWriteRepository, CustomerWriteRepository>();

            serviceCollection.AddScoped<IOrderReadRepository, OrderReadRepository>();
            serviceCollection.AddScoped<IOrderWriteRepository, OrderWriteRepository>();

            serviceCollection.AddScoped<IProductReadRepository, ProductReadRepository>();
            serviceCollection.AddScoped<IProductWriteRepository, ProductWriteRepository>();

            serviceCollection.AddScoped<IFileReadRepository, FileReadRepository>();
            serviceCollection.AddScoped<IFileWriteRepository, FileWriteRepository>();

            serviceCollection.AddScoped<IInvoiceImageFileReadRepository, InvoiceImageFileReadRepository>();
            serviceCollection.AddScoped<IInvoiceImageFileWriteRepository, InvoiceImageFileWriteRepository>();

            serviceCollection.AddScoped<IProductImageFileReadRepository, ProductImageFileReadRepository>();
            serviceCollection.AddScoped<IProductImageFileWriteRepository, ProductImageFileWriteRepository>();

            serviceCollection.AddScoped<IEndpointReadRepository, EndpointReadRepository>();
            serviceCollection.AddScoped<IEndpointWriteRepository, EndpointWriteRepository>();

            serviceCollection.AddScoped<IMenuReadRepository, MenuReadRepository>();
            serviceCollection.AddScoped<IMenuWriteRepository, MenuWriteRepository>();

            serviceCollection.AddScoped<IUserService, UserService>();
            serviceCollection.AddScoped<IAuthService, AuthService>();
            serviceCollection.AddScoped<IExternalAuthentication, AuthService>();
            serviceCollection.AddScoped<IInternalAuthentication, AuthService>();

            serviceCollection.AddScoped<IRoleService, RoleService>();

            serviceCollection.AddScoped<IAuthorizationEndpointService, AuthorizationEndpointService>();


        }
    }
}
