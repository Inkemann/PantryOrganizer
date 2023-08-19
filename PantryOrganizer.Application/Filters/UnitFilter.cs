using PantryOrganizer.Application.Dtos;
using PantryOrganizer.Application.Query;
using PantryOrganizer.Data.Models;

namespace PantryOrganizer.Application.Filters;

public class UnitFilter : AbstractFilter<UnitFilterDto, Unit>
{
    public UnitFilter() { }
}
