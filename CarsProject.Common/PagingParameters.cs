public class PagingParameters
{
    public int PageNumber { get; set; } = 1;

        private int _pageSize;
        
    public int PageSize
    {
        get => _pageSize == 0 ? 5 : _pageSize;
        set => _pageSize = value;
    }

    public int Skip => (PageNumber - 1) * PageSize;
   
    public static PagingParameters Lookup(int pageSize = 1000)
    {
        return new PagingParameters { PageNumber = 1, PageSize = pageSize };
    }
}

