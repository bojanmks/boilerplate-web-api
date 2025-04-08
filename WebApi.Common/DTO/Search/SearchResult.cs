using WebApi.Common.DTO.Search;

namespace WebApi.Application.Search
{
    public class SearchResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public PaginationSettings PaginationSettings { get; set; }
        public bool IsPaginated => PaginationSettings is not null;
    }
}