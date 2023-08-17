namespace PantryOrganizer.Application.Query;

public record Pagination : IPagination
{
    public int Page { get; set; }
    public int ItemsPerPage { get; set; }
}
