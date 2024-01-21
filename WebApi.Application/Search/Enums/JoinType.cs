using WebApi.Application.Search.Attributes;

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
