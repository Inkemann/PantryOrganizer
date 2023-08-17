namespace PantryOrganizer.Application.Query;

public record SortingParameter
{
    public SortingParameter() : this(SortingDirection.None, default)
    { }

    public SortingParameter(
        SortingDirection direction,
        int priority)
    {
        Direction = direction;
        Prio = priority;
    }

    public SortingDirection Direction { get; set; }
    public int Prio { get; set; }

    public bool IsEnabled => Direction != SortingDirection.None;
}

public enum SortingDirection
{
    None,
    Ascending,
    Descending,
}
