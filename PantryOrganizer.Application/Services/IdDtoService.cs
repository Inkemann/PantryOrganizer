using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PantryOrganizer.Application.Dtos;
using PantryOrganizer.Application.Extensions;
using PantryOrganizer.Application.Query;
using PantryOrganizer.Data;
using PantryOrganizer.Data.Models;

namespace PantryOrganizer.Application.Services;

public abstract class IdDtoService<TData, TDto, TId, TSorting, TFilter> :
    IDataService<TDto, TId, TSorting, TFilter>
    where TData : class, IIdEntity<TId>
    where TDto : class, IIdDto<TId>
    where TId : struct, IEquatable<TId>
{
    protected readonly PantryOrganizerContext context;
    protected readonly IMapper mapper;
    protected readonly IValidator<TDto> validator;
    protected readonly ISorter<TSorting, TData> sorting;
    protected readonly IFilter<TFilter, TData> filter;

    public delegate void EntityChangeHandler(object sender, EntityChangeEventArgs args);
    public event EntityChangeHandler? OnAddItem;
    public event EntityChangeHandler? OnUpdateItem;
    public event EntityChangeHandler? OnDeleteItem;

    public IdDtoService(
        PantryOrganizerContext context,
        IMapper mapper,
        IValidator<TDto> validator,
        ISorter<TSorting, TData> sorting,
        IFilter<TFilter, TData> filter)
    {
        this.context = context;
        this.mapper = mapper;
        this.validator = validator;
        this.sorting = sorting;
        this.filter = filter;
    }

    public IEnumerable<TDto> GetList(
        TFilter? filterData = default,
        TSorting? sortingData = default,
        IPagination? paginationData = default)
        => mapper.ProjectTo<TDto>(
                context.Set<TData>()
                    .Filter(filter, filterData)
                    .Sort(sorting, sortingData)
                    .Paginate(paginationData))
            .AsNoTrackingWithIdentityResolution()
            .AsEnumerable();

    public virtual EntityResult<TDto> GetById(TId id)
    {
        try
        {
            var entity = mapper.ProjectTo<TDto>(context.Set<TData>())
                .SingleOrDefault(item => item.Id.Equals(id));

            return new EntityResult<TDto>(entity);
        }
        catch (Exception ex)
        {
            return new EntityResult<TDto>(ex);
        }
    }

    public virtual EntityResult<TDto> Add(TDto item)
    {
        try
        {
            validator.ValidateAndThrow(item);

            var entity = mapper.Map<TDto, TData>(item);

            var eventArgs = new EntityChangeEventArgs(
                EntityChangeEventArgs.EntityChangeType.Add,
                entity);
            OnAddItem?.Invoke(this, eventArgs);

            if (eventArgs.Cancel)
                return new EntityResult<TDto>(false, true);

            entity = eventArgs.Entity;
            context.Set<TData>().Add(entity);
            bool success = context.SaveChanges() > 0;

            var result = mapper.Map<TDto>(entity);
            return new EntityResult<TDto>(result, success, false);
        }
        catch (Exception ex)
        {
            return new EntityResult<TDto>(ex);
        }
    }

    public virtual EntityResult<TDto> Update(TDto item)
    {
        try
        {
            if (!context.Set<TData>().Any(entity => entity.Id.Equals(item.Id)))
                throw new KeyNotFoundException();

            validator.ValidateAndThrow(item);

            var entity = mapper.Map<TDto, TData>(item);

            var eventArgs = new EntityChangeEventArgs(
                EntityChangeEventArgs.EntityChangeType.Update,
                entity);
            OnUpdateItem?.Invoke(this, eventArgs);

            if (eventArgs.Cancel)
                return new EntityResult<TDto>(false, true);

            entity = eventArgs.Entity;

            var entry = context.ChangeTracker.Entries<TData>()
                .SingleOrDefault(x => x.Entity.Id.Equals(entity.Id));

            if (entry != null)
                entry.CurrentValues.SetValues(entity);
            else
                context.Set<TData>().Update(entity);

            bool success = context.SaveChanges() > 0;

            var result = mapper.Map<TDto>(entity);
            return new EntityResult<TDto>(result, success, false);
        }
        catch (Exception ex)
        {
            return new EntityResult<TDto>(ex);
        }
    }

    public virtual ActionResult Delete(TId id)
    {
        try
        {
            var entity = context.Set<TData>()
                .SingleOrDefault(item => item.Id.Equals(id))
                ?? throw new KeyNotFoundException();

            var eventArgs = new EntityChangeEventArgs(
                EntityChangeEventArgs.EntityChangeType.Delete,
                entity);
            OnDeleteItem?.Invoke(this, eventArgs);

            if (eventArgs.Cancel)
                return new ActionResult(false, true);

            var entry = context.Remove(eventArgs.Entity);
            bool success = context.SaveChanges() > 0;

            return new ActionResult(success, false);
        }
        catch (Exception ex)
        {
            return new ActionResult(ex);
        }
    }

    public class EntityChangeEventArgs : EventArgs
    {
        public EntityChangeType Type { get; }
        public TData Entity { get; }
        public bool Cancel { get; set; }

        public EntityChangeEventArgs(EntityChangeType type, TData entity)
        {
            Type = type;
            Entity = entity;
        }

        public enum EntityChangeType
        {
            Add,
            Update,
            Delete,
        }
    }
}
