using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Data.DBContext;
using Microsoft.EntityFrameworkCore;
/*using Data.Interfaces;
using Data.Implementation;
using Domain.Interfaces;
using Domain.Implementation;*/

namespace IOC
{
    public static class Dependency
    {
        // With this method we obtain the connection string to the DBcontext
        public static void InjectDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SalesDbContext>(options =>
            {
                // The "SQLString" parameter come from the "appsettings.json" file on the GoSales application
                options.UseSqlServer(configuration.GetConnectionString("SQLString"));
            });
        }
    }
}
