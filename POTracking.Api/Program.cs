using Microsoft.AspNetCore.Mvc;
using PoTracking.Abstractions;
using POTracking.Data.InMemory;
using POTracking.Services;

namespace POTracking.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AngularApp", policy =>
                {
                    policy
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            ConfigureDi(builder);

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors("AngularApp");

            app.MapControllers();

            app.Run();
        }

        private static void ConfigureDi(WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IPurchaseOrderService, PurchaseOrderService>();
            builder.Services.AddSingleton<IPurchaseOrderRepository, PurchaseOrderRepository>(); // Adding as singleton for in-memory storage. Can be replaced with a database implementation if needed
            builder.Services.AddSingleton<ICustomerService, CustomerService>();
            builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>(); // Adding as singleton for in-memory storage. Can be replaced with a database implementation if needed
        }
    }
}
