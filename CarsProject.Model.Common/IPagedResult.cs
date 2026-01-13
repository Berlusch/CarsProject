namespace CarsProject.WebApi.Common
{
    public interface IPagedResult<T>
    {
        IEnumerable<T> Items { get; set; }
        int TotalCount { get; set; }
        int PageNumber { get; set; }
        int PageSize { get; set; }
        int TotalPages { get; }
    }
}
