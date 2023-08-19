namespace PantryOrganizer.Application.Dtos;

public interface IIdDto<TId>
    where TId : struct, IEquatable<TId>
{
    public TId Id { get; set; }
}
