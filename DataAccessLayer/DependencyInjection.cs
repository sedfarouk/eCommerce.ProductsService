using DataAccessLayer.DatabaseContext;
using DataAccessLayer.Repository;
using DataAccessLayer.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        // Add Data Access Layer services to IoC container
        services.AddDbContext<ApplicationDbContext>(opt => opt.UseNpgsql(configuration.GetConnectionString("PostgresConnectionString")));
        
        // Register repository services
        services.AddScoped<IProductsRepository, ProductsRepository>();

        return services; 
    }
}