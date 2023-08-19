using AutoMapper;
using FluentValidation;
using PantryOrganizer.Application.Dtos;
using PantryOrganizer.Application.Query;
using PantryOrganizer.Data;
using PantryOrganizer.Data.Models;

namespace PantryOrganizer.Application.Services;

public class UnitService :
    IdDtoService<Unit, UnitDto, Guid, UnitSortingDto, UnitFilterDto>,
    IUnitService
{
    public UnitService(
        PantryOrganizerContext context,
        IMapper mapper,
        IValidator<UnitDto> validator,
        ISorter<UnitSortingDto, Unit> sorting,
        IFilter<UnitFilterDto, Unit> filter)
        : base(context, mapper, validator, sorting, filter)
    { }
}
