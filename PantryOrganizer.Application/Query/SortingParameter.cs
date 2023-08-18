namespace PantryOrganizer.Application.Query;

public record SortingParameter(SortingDirection? Direction, int Priority)
{
    public SortingParameter() : this(null, 0) { }

    public SortingParameter(SortingDirection direction) : this(direction, 0) { }

    public bool IsEnabled => Direction.HasValue;
}

public enum SortingDirection
{
    Ascending,
    Descending,
}
