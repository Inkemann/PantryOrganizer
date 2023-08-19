using PantryOrganizer.Application.Dtos;
using PantryOrganizer.Application.Query;
using PantryOrganizer.Data.Models;

namespace PantryOrganizer.Application.Sorters;

public class UnitSorter : AbstractSorter<UnitSortingDto, Unit>
{
    public UnitSorter()
    {
        SortBy(unit => unit.Dimension)
            .Using(sorting => sorting.Dimension)
            .AsDefault(priority: 0);
        SortBy(unit => unit.BaseConversionFactor)
            .Using(sorting => sorting.ConversionFactor)
            .AsDefault(priority: 1);
    }
}
