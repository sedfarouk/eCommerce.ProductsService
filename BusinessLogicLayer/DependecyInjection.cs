using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Mappers;
using BusinessLogicLayer.ServiceContracts;
using BusinessLogicLayer.Services;
using BusinessLogicLayer.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogicLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
    {
        // Add Data Access Layer services to IoC container
        services.AddAutoMapper(cfg => { }, typeof(ProductMappingProfile).Assembly);
        services.AddScoped<IProductsService, ProductsService>();
        services.AddValidatorsFromAssemblyContaining<AddProductRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateProductRequestValidator>();
        
        return services; 
    }
}