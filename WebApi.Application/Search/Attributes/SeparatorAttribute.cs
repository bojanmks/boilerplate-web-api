using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Application.Search.Attributes
{
    public class SeparatorAttribute : Attribute
    {
        private readonly string _separator;

        public SeparatorAttribute(string separator)
        {
            _separator = separator;
        }

        public string Separator => _separator;
    }
}
