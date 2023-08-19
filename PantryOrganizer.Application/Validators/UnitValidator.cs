using FluentValidation;
using PantryOrganizer.Application.Dtos;

namespace PantryOrganizer.Application.Validators;

public class UnitValidator : AbstractValidator<UnitDto>
{
    public UnitValidator()
    {
        RuleFor(unit => unit.BaseConversionFactor)
            .GreaterThan(0d)
            .When(unit => unit.BaseConversionFactor.HasValue);
        RuleFor(unit => unit.Abbreviation)
            .NotEmpty()
            .MaximumLength(StringLength.Medium);
        RuleFor(unit => unit.Name)
            .NotEmpty()
            .MaximumLength(StringLength.Medium);
        RuleFor(unit => unit.AbbreviationPlural)
            .NotEmpty()
            .MaximumLength(StringLength.Medium);
        RuleFor(unit => unit.NamePlural)
            .NotEmpty()
            .MaximumLength(StringLength.Medium);
        RuleFor(unit => unit.Dimension)
            .IsInEnum()
            .When(unit => unit.Dimension.HasValue);
    }
}
