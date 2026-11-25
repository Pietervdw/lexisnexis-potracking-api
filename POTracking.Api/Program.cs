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

            // I like to shape my API response in a consistent way. Not necessary, but I think it makes the API more user-friendly
            // This way the data attribute is still used and we add our own erroes list.
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => string.IsNullOrEmpty(e.ErrorMessage)
                            ? "Invalid request data."
                            : e.ErrorMessage)
                        .ToList();

                    return new BadRequestObjectResult(new ValidationError
                    {
                        Title = "Validation failed",
                        Status = StatusCodes.Status400BadRequest,
                        Success = false,
                        Errors = errors
                    });
                };
            });

            ConfigureDi(builder);

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        private static void ConfigureDi(WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IPurchaseOrderService, PurchaseOrderService>();
            builder.Services.AddSingleton<IPurchaseOrderRepository, PurchaseOrderRepository>(); // Adding as singleton for in-memory storage. Can we replaced with a database implementation if needed
            builder.Services.AddSingleton<ICustomerService, CustomerService>();
            builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>(); // Adding as singleton for in-memory storage. Can we replaced with a database implementation if needed
        }
    }
}
