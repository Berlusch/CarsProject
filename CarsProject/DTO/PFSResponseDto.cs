namespace CarsProject.WebApi.DTO
{
    
        public class PFSResponseDto<T>
        {
            public IEnumerable<T> Items { get; set; } = new List<T>();
            public bool HasNextPage { get; set; }
            public int TotalCount { get; set; }
        }
    
}
