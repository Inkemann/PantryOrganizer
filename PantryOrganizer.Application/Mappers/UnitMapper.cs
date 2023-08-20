using AutoMapper;
using PantryOrganizer.Application.Dtos;
using PantryOrganizer.Data.Models;

namespace PantryOrganizer.Application.Mappers;

public class UnitMapper : Profile
{
    public UnitMapper()
    {
        CreateMap<Unit, UnitDto>()
            .ForMember(
                unit => unit.Dimension,
                options => options.MapFrom(unit => (UnitDimensionEnumDto?)unit.DimensionId));
        CreateMap<UnitDto, Unit>()
            .ForMember(
                unit => unit.DimensionId,
                options => options.MapFrom(unit => (UnitDimensionEnum?)unit.Dimension));
    }
}
