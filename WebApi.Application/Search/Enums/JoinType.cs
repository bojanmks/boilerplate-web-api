using WebApi.Application.Search.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Application.Search.Enums
{
    public enum JoinType
    {
        [Separator(" + ")]
        Basic = 1,
        [Separator(" + \" \" + ")]
        WithSpaces = 2
    }
}
