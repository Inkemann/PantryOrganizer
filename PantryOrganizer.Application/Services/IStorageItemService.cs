using PantryOrganizer.Application.Dtos;

namespace PantryOrganizer.Application.Services;

public interface IStorageItemService :
    IDataService<StorageItemDto, Guid, StorageItemSortingDto, StorageItemFilterDto>
{ }
