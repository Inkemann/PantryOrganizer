using AutoMapper;
using FluentValidation;
using PantryOrganizer.Application.Dtos;
using PantryOrganizer.Application.Query;
using PantryOrganizer.Data;
using PantryOrganizer.Data.Models;

namespace PantryOrganizer.Application.Services;

public class StorageItemService :
    IdDtoService<StorageItem, StorageItemDto, Guid, StorageItemSortingDto, StorageItemFilterDto>,
    IStorageItemService
{
    public StorageItemService(
        PantryOrganizerContext context,
        IMapper mapper,
        IValidator<StorageItemDto> validator,
        ISorter<StorageItemSortingDto, StorageItem> sorter,
        IFilter<StorageItemFilterDto, StorageItem> filter)
        : base(context, mapper, validator, sorter, filter)
        => OnAddItem += ItemAdded;

    private void ItemAdded(object sender, EntityChangeEventArgs args)
        => args.Entity.StoredDate = DateTime.Now;
}
