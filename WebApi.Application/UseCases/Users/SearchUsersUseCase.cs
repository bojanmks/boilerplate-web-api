using WebApi.Application.Search;
using WebApi.Application.UseCases.Attributes;
using WebApi.Common.Enums;
using WebApi.DataAccess.Entities;

namespace WebApi.Application.UseCases.Users
{
    [AllowForRoles(UserRole.Admin)]
    public class SearchUsersUseCase : UseCase<ISearchObject<User>, object>
    {
        public SearchUsersUseCase(ISearchObject<User> data) : base(data)
        {
        }

        public override string Id => "SearchUsers";
    }
}
