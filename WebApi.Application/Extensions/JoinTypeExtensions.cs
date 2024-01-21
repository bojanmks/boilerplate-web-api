using WebApi.Application.Search.Attributes;
using WebApi.Application.Search.Enums;
using WebApi.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Application.Extensions
{
    public static class JoinTypeExtensions
    {
        public static string GetSeparator(this JoinType joinType)
        {
            var separatorAttribute = joinType.GetAttributeOfType<SeparatorAttribute>();

            return separatorAttribute?.Separator ?? " + ";
        }
    }
}
