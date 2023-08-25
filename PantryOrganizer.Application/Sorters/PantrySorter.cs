using PantryOrganizer.Application.Dtos;
using PantryOrganizer.Application.Query;
using PantryOrganizer.Data.Models;

namespace PantryOrganizer.Application.Sorters;

public class PantrySorter : AbstractSorter<PantrySortingDto, Pantry>
{
    public PantrySorter()
    {
        SortBy(pantry => pantry.Name)
            .Using(sorting => sorting.Name)
            .AsDefault();
        SortBy(pantry => pantry.Items.Count())
            .Using(sorting => sorting.StorageItemsCount);
    }
}
