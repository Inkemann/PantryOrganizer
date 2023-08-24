using PantryOrganizer.Application.AuxiliaryModels;
using PantryOrganizer.Application.Dtos;
using PantryOrganizer.Application.Extensions;
using PantryOrganizer.Application.Query;
using PantryOrganizer.Data.Models;

namespace PantryOrganizer.Application.Filters;

public class StorageItemFilter : AbstractFilter<StorageItemFilterDto, StorageItem>
{
    public StorageItemFilter()
        => FilterFor(storageItem => storageItem.Name)
            .Contains(filter => filter.Name)
            .IgnoreWhitespace();
}

public class StorageItemGroupFilter :
    AbstractFilter<StorageItemGroupFilterDto, StorageItemGroup>
{
    public StorageItemGroupFilter()
        => FilterFor(storageItemGroup => storageItemGroup.Name)
            .Contains(filter => filter.Name)
            .IgnoreWhitespace();
}
