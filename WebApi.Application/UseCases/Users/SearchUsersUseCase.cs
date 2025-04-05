using WebApi.Application.Search;
using WebApi.Common.Enums.Auth;

namespace WebApi.Application.UseCases.Users
{
    [AllowForRoles(UserRole.Admin)]
    public class SearchUsersUseCase : UseCase<ISearchObject, object>
    {
        public SearchUsersUseCase(ISearchObject data) : base(data)
        {
        }

        public override string Id => "SearchUsers";
    }
}
