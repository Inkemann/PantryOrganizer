using FluentValidation;
using PantryOrganizer.Application.Dtos;

namespace PantryOrganizer.Application.Validators;

public class PantryValidator : AbstractValidator<PantryDto>
{
    public PantryValidator()
        => RuleFor(pantry => pantry.Name)
            .NotEmpty()
            .MaximumLength(StringLength.Medium);
}
