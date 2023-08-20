using PantryOrganizer.Application.Dtos;

namespace PantryOrganizer.Application.Services;

public interface IUnitService :
    IDataService<UnitDto, Guid, UnitSortingDto, UnitFilterDto>
{
    public ConversionResult GetConversionRate(Guid fromId, Guid toId);

    public ConversionResult GetBaseConversion(Guid unitId);

    public record ConversionResult(UnitDto? Base, double? ConversionRate)
    {
        public ConversionResult() : this(null, null) { }
    }
}
