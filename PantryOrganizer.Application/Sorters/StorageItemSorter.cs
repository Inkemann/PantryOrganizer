using PantryOrganizer.Application.AuxiliaryModels;
using PantryOrganizer.Application.Dtos;
using PantryOrganizer.Application.Query;
using PantryOrganizer.Data.Models;

namespace PantryOrganizer.Application.Sorters;

public class StorageItemSorter : AbstractSorter<StorageItemSortingDto, StorageItem>
{
    public StorageItemSorter()
        => SortBy(storageItem => storageItem.Name)
            .Using(sorting => sorting.Name)
            .AsDefault();
}

public class StorageItemGroupSorter :
    AbstractSorter<StorageItemGroupSortingDto, StorageItemGroup>
{
    public StorageItemGroupSorter()
        => SortBy(storageItem => storageItem.Name)
            .Using(sorting => sorting.Name)
            .AsDefault();
}
