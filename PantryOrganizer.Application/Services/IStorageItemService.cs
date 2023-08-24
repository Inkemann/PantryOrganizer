using PantryOrganizer.Application.Dtos;
using PantryOrganizer.Application.Query;

namespace PantryOrganizer.Application.Services;

public interface IStorageItemService :
    IDataService<StorageItemDto, Guid, StorageItemSortingDto, StorageItemFilterDto>
{
    public IEnumerable<StorageItemGroupDto> GetListGrouped(
        StorageItemGroupFilterDto? filter = default,
        StorageItemGroupSortingDto? sorting = default,
        IPagination? pagination = default);

    public IEnumerable<StorageItemDto> GetItemsOfGroup(
        string? name,
        UnitDimensionEnumDto? dimensionId,
        Guid? pantryId);
}
