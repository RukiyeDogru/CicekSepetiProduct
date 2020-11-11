using CicekSepeti.Domain.Services.Impl.Services;
using CicekSepeti.Domain.Services.Services;
using CicekSepeti.Infra.Data.Database;
using CicekSepeti.Infra.Data.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace CicekSepeti.Infra.CrossCutting.IoC
{
   public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<CicekSepetiContext>();
        }
      
    }
}
