using CarsProject.Model.Common;

namespace CarsProject.Model
{
    public class PagedResult<T> : IPagedResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        // Izračunaj TotalPages na temelju TotalCount i PageSize
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        // Konstruktor koji inicijalizira Items kao praznu listu ako nije proslijeđena
        public PagedResult()
        {
            Items = new List<T>();  // Osiguravamo da Items nije null
        }
    }
}
