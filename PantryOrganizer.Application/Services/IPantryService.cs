using PantryOrganizer.Application.Dtos;

namespace PantryOrganizer.Application.Services;

public interface IPantryService :
    IDataService<PantryDto, Guid, PantrySortingDto, PantryFilterDto>
{ }
