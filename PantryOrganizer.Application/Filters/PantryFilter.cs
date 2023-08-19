using PantryOrganizer.Application.Dtos;
using PantryOrganizer.Application.Extensions;
using PantryOrganizer.Application.Query;
using PantryOrganizer.Data.Models;

namespace PantryOrganizer.Application.Filters;

public class PantryFilter : AbstractFilter<PantryFilterDto, Pantry>
{
    public PantryFilter()
        => FilterFor(pantry => pantry.Name)
            .Contains(filter => filter.Name)
            .IgnoreWhitespace();
}
