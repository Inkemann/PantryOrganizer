namespace PantryOrganizer.Application.Query;

public record SortingParameter(SortingDirection Direction, int Prio)
{
    public bool IsEnabled => Direction != SortingDirection.None;
}

public enum SortingDirection
{
    None,
    Ascending,
    Descending,
}
