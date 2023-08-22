using AutoMapper;
using FluentValidation;
using PantryOrganizer.Application.Dtos;
using PantryOrganizer.Application.Query;
using PantryOrganizer.Data;
using PantryOrganizer.Data.Models;

namespace PantryOrganizer.Application.Services;

public class PantryService :
    IdDtoService<Pantry, PantryDto, Guid, PantrySortingDto, PantryFilterDto>,
    IPantryService
{
    public PantryService(
        PantryOrganizerContext context,
        IMapper mapper,
        IValidator<PantryDto> validator,
        ISorter<PantrySortingDto, Pantry> sorter,
        IFilter<PantryFilterDto, Pantry> filter)
        : base(context, mapper, validator, sorter, filter)
    { }
}
