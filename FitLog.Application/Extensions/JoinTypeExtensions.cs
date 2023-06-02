using FitLog.Application.Search.Attributes;
using FitLog.Application.Search.Enums;
using FitLog.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Application.Extensions
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
