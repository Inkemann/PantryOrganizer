namespace PantryOrganizer.Data.Models;

public interface IIdEntity<TId>
    where TId : struct, IEquatable<TId>
{
    public TId Id { get; set; }
}
