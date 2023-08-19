using AutoMapper;
using PantryOrganizer.Application.Dtos;
using PantryOrganizer.Data.Models;

namespace PantryOrganizer.Application.Mappers;

internal class PantryMapper : Profile
{
    public PantryMapper()
    {
        CreateMap<Pantry, PantryDto>();
        CreateMap<PantryDto, Pantry>();
    }
}
