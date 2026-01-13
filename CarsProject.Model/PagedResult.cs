using CarsProject.WebApi.Common;

namespace CarsProject.WebApi
{
    public class PagedResult<T> : IPagedResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
               
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        
        public PagedResult()
        {
            Items = new List<T>();  
        }
    }
}
