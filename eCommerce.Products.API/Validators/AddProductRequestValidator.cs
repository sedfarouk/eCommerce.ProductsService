using eCommerce.Products.API.Models.DTO;
using FluentValidation;

namespace eCommerce.Products.API.Validators;

public class AddProductRequestValidator : AbstractValidator<AddProductRequestDto> 
{
    public AddProductRequestValidator()
    {
        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage("Product name cannot be empty")
            .Length(1, 50).WithMessage("Product name must be between 1 and 50 characters");
        RuleFor(x => x.Category)
            .IsInEnum().WithMessage("Category must be a valid category")
            .NotEmpty().WithMessage("Category cannot be empty");
        RuleFor(x => x.QuantityInStock)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0")
            .NotEmpty().WithMessage("Quantity cannot be empty");
        RuleFor(x => x.UnitPrice)
            .GreaterThan(0).WithMessage("Unit price must be greater than 0");
    }
}