using BusinessLogicLayer.DTO;
using FluentValidation;

namespace BusinessLogicLayer.Validators;

public class AddProductRequestValidator : AbstractValidator<AddProductRequestDto> 
{
    public AddProductRequestValidator()
    {
        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage("Product name cannot be empty")
            .Length(1, 50).WithMessage("Product name must be between 1 and 50 characters");
        RuleFor(x => x.Category)
            .IsInEnum().WithMessage("Category must be a valid category");
        RuleFor(x => x.QuantityInStock)
            .InclusiveBetween(0, int.MaxValue).WithMessage("Quantity out of range");
        RuleFor(x => x.UnitPrice)
            .InclusiveBetween(0, int.MaxValue).WithMessage("Unit price out of range");
    }
}