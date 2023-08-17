namespace PantryOrganizer.Application.Query;

public interface IPagination
{
    public int Page { get; set; }
    public int ItemsPerPage { get; set; }
}
