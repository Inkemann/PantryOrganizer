using FluentValidation;
using PantryOrganizer.Application.Dtos;

namespace PantryOrganizer.Application.Validators;

public class StorageItemValidator : AbstractValidator<StorageItemDto>
{
    public StorageItemValidator()
    {
        RuleFor(storageItem => storageItem.Name)
            .NotEmpty()
            .MaximumLength(StringLength.Medium);
        RuleFor(storageItem => storageItem.Note)
            .MaximumLength(StringLength.Long);
        RuleFor(storageItem => storageItem.Quantity)
            .NotEmpty()
            .GreaterThanOrEqualTo(0);
        RuleFor(storageItem => storageItem.UnitId)
            .NotEmpty();
        RuleFor(storageItem => storageItem.RemainingPercentage)
            .InclusiveBetween(0, 1)
            .When(storageItem => storageItem.RemainingPercentage.HasValue);
        RuleFor(storageItem => storageItem.PantryId)
            .NotEmpty();
    }
}
