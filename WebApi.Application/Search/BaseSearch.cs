namespace WebApi.Application.Search
{
    public abstract class BaseSearch : ISearchObject
    {
        public int Page { get; set; } = 1;
        public int PerPage { get; set; } = 10;
        public bool Paginate { get; set; } = false;
        public string SortBy { get; set; } = string.Empty;
    }
}
