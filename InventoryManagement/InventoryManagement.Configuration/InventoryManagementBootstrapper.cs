using InventoryManagement.Application.ApplicationSercices;
using InventoryManagement.Application.DomainServices;
using InventoryManagement.Domain.Models;
using InventoryManagement.Domain.Models.InventoryAgg;
using InventoryManagement.Infrastructure.Persistent.EF.Context;
using InventoryManagement.Infrastructure.Persistent.EF.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace InventoryManagement.Configuration
{
    public class InventoryManagementBootstrapper
    {
        public static void Init(IServiceCollection service, string connectionString)
        {
            service.AddTransient<IInventoryRepository, InventoryRepository>();
            service.AddDbContext<InventoryContext>(option =>
            {
                option.UseSqlServer(connectionString);
            });
            service.AddScoped<IInventoryService, InventoryService>();
            service.AddScoped<IInventoryDomainService, InventoryDomainService>();
        }
    }
}