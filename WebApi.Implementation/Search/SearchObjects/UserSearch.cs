using WebApi.Application.Search;
using WebApi.DataAccess.Entities;

namespace WebApi.Implementation.Search.SearchObjects
{
    public class UserSearch : BaseSearch<User>
    {
        public UserSearch()
        {
            AddFilterProperty(
                () => Name,
                (name) => (x) => (x.FirstName + " " + x.LastName).Contains((string)name)
            );

            AddFilterProperty(
                () => Email,
                (name) => (x) => x.Email.Contains((string)name)
            );

            AddSortByProperty(
                "name",
                (x) => x.FirstName + " " + x.LastName
            );

            AddSortByProperty(
                "email",
                (x) => x.Email
            );
        }

        public string? Name { get; set; }
        public string? Email { get; set; }
    }
}
