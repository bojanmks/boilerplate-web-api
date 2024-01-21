using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Application.Search
{
    public interface ISearchObject
    {
        public int Page { get; set; }
        public int PerPage { get; set; }
        public bool Paginate { get; set; }
        public string SortBy { get; set; }
        public Dictionary<string, string> CustomSortBy { get; set; }
    }
}
