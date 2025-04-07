using WebApi.Application.Search;
using WebApi.DataAccess.Entities;

namespace WebApi.Implementation.Search.SearchObjects
{
    public class UserSearch : EfSearch<User>
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

            DefineSortProperty(
                "name",
                (x) => x.FirstName + " " + x.LastName
            );

            DefineSortProperty(
                "email",
                (x) => x.Email
            );
        }

        public string? Name { get; set; }
        public string? Email { get; set; }
    }
}
