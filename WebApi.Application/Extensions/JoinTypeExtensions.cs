using WebApi.Application.Search.Attributes;
using WebApi.Application.Search.Enums;
using WebApi.Common.Extensions;

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
