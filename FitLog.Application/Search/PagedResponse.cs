using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Application.Search
{
    public class PagedResponse<T>
    {
        public int Page { get; set; }
        public int PerPage { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
