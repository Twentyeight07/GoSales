using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Data.DBContext;
using Microsoft.EntityFrameworkCore;
using Data.Interfaces;
using Data.Implementation;
using Domain.Interfaces;
using Domain.Implementation;

namespace IOC
{
    public static class Dependency
    {
        // With this method we inyect dependencies to the application
        public static void InjectDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SalesDbContext>(options =>
            {
                // The "SQLString" parameter come from the "appsettings.json" file on the GoSales application
                options.UseSqlServer(configuration.GetConnectionString("SQLString"));
            });
            
            // This registers the 'GenericRepository<T>' implementation for the 'IGenericRepository<T>' interface. This allows for easy implementation of CRUD (Create, Read, Update, Delete) operations for any type T using the IGenericRepository interface.
            services.AddTransient(typeof(IGenericRepository<>),typeof(GenericRepository<>));
            // This registers the 'SaleRepository' implementation for the 'ISaleRepository' interface. This allows for specific implementation of CRUD operations and any other business logic related to sales.
            services.AddScoped<ISaleRepository, SaleRepository>();

            // This registers the 'EmailService' implementation for the 'IEmailService' interface. This allows for sending email messages from the application using the IEmailService interface.
            services.AddScoped<IEmailService, EmailService>();

            // This registers the 'FireBaseService' implementation for the 'IFireBaseService' interface. This allows for integration with Firebase services from the application using the IFireBaseService interface.
            services.AddScoped<IFireBaseService, FireBaseService>();

            // This registers the 'UtilitiesService' implementation for the 'IUtilitiesService' interface. This allows for various utility functions to be used throughout the application using the IUtilitiesService interface.
            services.AddScoped<IUtilitiesService, UtilitiesService>();

            // This registers the 'RoleService' implementation for the 'IRolesService' interface. This allows for specific implementation of business logic related to user roles and permissions.
            services.AddScoped<IRolesService, RoleService>();

            // This registers the 'UserService' implementation for the 'IUserService' interface. This allows for specific implementation of CRUD operations and any other business logic related to users.
            services.AddScoped<IUserService, UserService>();

            // This registers the 'BusinessService' implementation for the 'IBusinessService' interface.
            services.AddScoped<IBusinessService, BusinessService>();

            // This registers the 'CategoryService' implementation for the 'ICategoryService' interface. This allows for specific implementation of CRUD operations and any other business logic related to Categories.
            services.AddScoped<ICategoryService, CategoryService>();

            // This registers the 'ProductService' implementation for the 'IProductService' interface. This allows for specific implementation of CRUD operations and any other business logic related to Products.
            services.AddScoped<IProductService, ProductService>();

            // This registers the 'SaleDocTypeService' implementation for the 'ISaleDocTypeService' interface. This allows consult of the Sale Doc Type.
            services.AddScoped<ISaleDocTypeService, SaleDocTypeService>();

            // This registers the 'SaleService' implementation for the 'ISaleService' interface. This allows for specific implementation of CRUD operations and any other business logic related to Sales.
            services.AddScoped<ISaleService, SaleService>();

            // This registers the 'DashBoardService' implementation for the 'IDashBoardService' interface. This allows for specific implementation of Consult operations and any other business logic related to the DashBoard.
            services.AddScoped<IDashBoardService, DashBoardService>();

            // This registers the 'MenuService' implementation for the 'IMenuService' interface. This allows the user to access only to the menus he's supose to access.
            services.AddScoped<IMenuService, MenuService>();


            services.AddScoped<INotificationService, NotificationService>();
        }
    }
}
