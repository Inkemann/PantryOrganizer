using AutoMapper;
using FluentValidation;
using PantryOrganizer.Application.AuxiliaryModels;
using PantryOrganizer.Application.Dtos;
using PantryOrganizer.Application.Extensions;
using PantryOrganizer.Application.Query;
using PantryOrganizer.Data;
using PantryOrganizer.Data.Models;

namespace PantryOrganizer.Application.Services;

public class StorageItemService :
    IdDtoService<StorageItem, StorageItemDto, Guid, StorageItemSortingDto, StorageItemFilterDto>,
    IStorageItemService
{
    protected readonly ISorter<StorageItemGroupSortingDto, StorageItemGroup> groupSorter;
    protected readonly IFilter<StorageItemGroupFilterDto, StorageItemGroup> groupFilter;

    public StorageItemService(
        PantryOrganizerContext context,
        IMapper mapper,
        IValidator<StorageItemDto> validator,
        ISorter<StorageItemSortingDto, StorageItem> sorter,
        IFilter<StorageItemFilterDto, StorageItem> filter,
        ISorter<StorageItemGroupSortingDto, StorageItemGroup> groupSorter,
        IFilter<StorageItemGroupFilterDto, StorageItemGroup> groupFilter)
        : base(context, mapper, validator, sorter, filter)
    {
        this.groupFilter = groupFilter;
        this.groupSorter = groupSorter;

        OnAddItem += ItemAdded;
    }
    private void ItemAdded(object sender, EntityChangeEventArgs args)
        => args.Entity.StoredDate = DateTime.Now;

    public IEnumerable<StorageItemGroupDto> GetListGrouped(
        StorageItemGroupFilterDto? filter = null,
        StorageItemGroupSortingDto? sorting = null,
        IPagination? pagination = null)
    {
        var groupQuery = context.Set<StorageItem>()
                .Where(item => item.Unit != default)
                .GroupBy(item => new
                {
                    item.Name,
                    item.Unit!.DimensionId,
                    item.PantryId
                })
                .Select(group => new StorageItemGroup
                {
                    ItemCount = group.Count(),
                    Name = group.Key.Name,
                    Quantity = group.Sum(item =>
                        item.Quantity * (decimal)item.Unit!.BaseConversionFactor!
                        * (decimal)(item.RemainingPercentage ?? 1d)),
                    Unit = context.Set<Unit>()
                        .Single(unit =>
                            unit.DimensionId == group.Key.DimensionId && unit.IsBase),
                    PantryId = group.First().Pantry!.Id,
                })
                .Sort(groupSorter, sorting)
                .Filter(groupFilter, filter)
                .Paginate(pagination);

        return mapper.Map<IEnumerable<StorageItemGroupDto>>(
            groupQuery.AsEnumerable());
    }

    public IEnumerable<StorageItemDto> GetItemsOfGroup(
        string? name,
        UnitDimensionEnumDto? dimensionId,
        Guid? pantryId)
        => name == default || dimensionId == null || pantryId == default
            ? Enumerable.Empty<StorageItemDto>()
            : mapper.ProjectTo<StorageItemDto>(
                context.Set<StorageItem>()
                    .Where(item =>
                        item.Name == name
                        && item.Unit!.DimensionId == (UnitDimensionEnum)dimensionId
                        && item.PantryId == pantryId));
}
