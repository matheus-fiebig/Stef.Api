using API.STEF.Application.Orders.Services;
using API.STEF.Data.Repositories;
using API.STEF.Data.UOW;
using API.STEF.Domain.Contracts.Repositories;
using API.STEF.Domain.Contracts.UnitOfWork;
using API.STEF.Domain.ProductAggregator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace API.STEF.Ioc
{
    public static class Bootstrap
    {
        public static void InjectDependecy(this IServiceCollection services)
        {
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static IHost Migrate<T>(this IHost webHost) where T : DbContext
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var db = services.GetRequiredService<T>();
                    db.Database.Migrate();

                    if(!db.Set<Product>().Any())
                    {
                        db.Set<Product>().Add(Product.CreateNew("Camisa", 100));
                        db.Set<Product>().Add(Product.CreateNew("Oculos", 10.50m));
                        db.SaveChanges();
                    }
                }
                catch(Exception ex) 
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine(ex.InnerException?.Message);
                    throw;
                }
            }

            return webHost;
        }
    }
}
