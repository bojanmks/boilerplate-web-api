using System.Linq.Expressions;
using WebApi.DataAccess.Entities.Abstraction;

namespace WebApi.Application.Search
{
    public interface ISearchObject
    {
        public int Page { get; set; }
        public int PerPage { get; set; }
        public bool Paginate { get; set; }
        public string SortBy { get; set; }
    }
}
