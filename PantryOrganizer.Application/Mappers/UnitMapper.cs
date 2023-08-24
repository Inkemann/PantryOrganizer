using AutoMapper;
using PantryOrganizer.Application.Dtos;
using PantryOrganizer.Data.Models;

namespace PantryOrganizer.Application.Mappers;

public class UnitMapper : Profile
{
    public UnitMapper()
    {
        CreateMap<Unit, UnitDto>();
        CreateMap<UnitDto, Unit>();
    }
}
