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

    public IUnitService.ConversionResult GetConversionRate(Guid fromId, Guid toId)
    {
        var from = context.Set<Unit>().SingleOrDefault(item => item.Id == fromId);
        var to = context.Set<Unit>().SingleOrDefault(item => item.Id == toId);

        if (from == null || to == null
            || !from.DimensionId.HasValue || from.DimensionId != to.DimensionId)
        {
            return new IUnitService.ConversionResult();
        }

        if (from.IsBase)
        {
            return new IUnitService.ConversionResult(
                mapper.Map<UnitDto>(from),
                1d / to.BaseConversionFactor);
        }
        if (to.IsBase)
        {
            return new IUnitService.ConversionResult(
                mapper.Map<UnitDto>(to),
                from.BaseConversionFactor);
        }

        var baseUnit = context.Set<Unit>()
            .SingleOrDefault(item => item.IsBase && item.DimensionId == from.DimensionId);
        double? fromConversionRate = from.BaseConversionFactor;
        double? toConversionRate = 1d / to.BaseConversionFactor;

        return new IUnitService.ConversionResult(
            mapper.Map<UnitDto>(baseUnit),
            fromConversionRate * toConversionRate);
    }

    public IUnitService.ConversionResult GetBaseConversion(Guid unitId)
    {
        var unit = context.Set<Unit>().SingleOrDefault(item => item.Id == unitId);

        if (unit == null || !unit.DimensionId.HasValue)
            return new IUnitService.ConversionResult();

        var baseUnit = unit.IsBase
            ? unit
            : context.Set<Unit>()
                .SingleOrDefault(item => item.IsBase && item.DimensionId == unit.DimensionId);

        return new IUnitService.ConversionResult(
            mapper.Map<UnitDto>(baseUnit),
            unit.BaseConversionFactor);
    }
}
