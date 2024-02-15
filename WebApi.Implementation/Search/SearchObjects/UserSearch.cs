using WebApi.Application.Search;
using WebApi.DataAccess.Entities;

namespace WebApi.Implementation.Search.SearchObjects
{
    public class UserSearch : BaseSearch<User>
    {
        public UserSearch()
        {
            DefineFilterProperty(
                () => Name,
                (name) => (x) => (x.FirstName + " " + x.LastName).Contains((string)name)
            );

            DefineFilterProperty(
                () => Email,
                (name) => (x) => x.Email.Contains((string)name)
            );

            DefineSortByProperty(
                "name",
                (x) => x.FirstName + " " + x.LastName
            );

            DefineSortByProperty(
                "email",
                (x) => x.Email
            );
        }

        public string? Name { get; set; }
        public string? Email { get; set; }
    }
}
