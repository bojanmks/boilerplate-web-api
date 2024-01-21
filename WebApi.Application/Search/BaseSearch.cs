using WebApi.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Application.Search
{
    public abstract class BaseSearch : ISearchObject
    {
        public int Page { get; set; } = 1;
        public int PerPage { get; set; } = 10;
        public bool Paginate { get; set; } = false;
        public string SortBy { get; set; } = "";
        public Dictionary<string, string> CustomSortBy { get; set; } = new Dictionary<string, string>();

        protected void AddCustomSortProperty(string key, string query)
        {
            if (CustomSortBy.ContainsKey(key))
            {
                throw new SortPropertyAlreadyExistsException(key);
            }

            CustomSortBy.Add(key, query);
        }
    }
}
