using PantryOrganizer.Application.Dtos;

namespace PantryOrganizer.Application.Services;

public interface IUnitService :
    IDataService<UnitDto, Guid, UnitSortingDto, UnitFilterDto>
{ }
