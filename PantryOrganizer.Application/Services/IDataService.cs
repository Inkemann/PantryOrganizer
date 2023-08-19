using PantryOrganizer.Application.Dtos;
using PantryOrganizer.Application.Query;

namespace PantryOrganizer.Application.Services;

public interface IDataService<TDto, TId, TSorting, TFilter> :
    IEntityService<TDto, TId>,
    IListService<TDto, TSorting, TFilter>
    where TDto : class, IIdDto<TId>
    where TId : struct, IEquatable<TId>
{ }

public interface IListService<TDto, TSorting, TFilter>
{
    public IEnumerable<TDto> GetList(
        TFilter? filter = default,
        TSorting? sortingData = default,
        IPagination? paginationData = default);
}

public interface IEntityService<TDto, TId>
    where TDto : class, IIdDto<TId>
    where TId : struct, IEquatable<TId>
{
    public EntityResult<TDto> GetById(TId id);

    public EntityResult<TDto> Add(TDto item);

    public EntityResult<TDto> Update(TDto item);

    public ActionResult Delete(TId id);
}

public record ActionResult(bool IsSuccess, bool IsCancelled, Exception? Exception)
{
    public ActionResult(bool isSuccess, bool isCancelled)
        : this(isSuccess, isCancelled, null)
    { }

    public ActionResult(Exception exception)
        : this(false, false, exception)
    { }
}

public record EntityResult<T>(
    T? Result,
    bool IsSuccess,
    bool IsCancelled,
    Exception? Exception)
    : ActionResult(IsSuccess, IsCancelled, Exception)
    where T : class
{
    public EntityResult(T? entity)
        : this(entity, entity != null, false, null)
    { }

    public EntityResult(T? entity, bool isSuccess, bool isCancelled)
        : this(entity, isSuccess, isCancelled, null)
    { }

    public EntityResult(bool isSuccess, bool isCancelled)
        : this(null, isSuccess, isCancelled, null)
    { }

    public EntityResult(Exception exception)
        : this(null, false, false, exception)
    { }
}
