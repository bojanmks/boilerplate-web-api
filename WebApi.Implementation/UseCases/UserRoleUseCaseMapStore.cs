using WebApi.Common.Enums.Auth;

namespace WebApi.Implementation.UseCases
{
    public class UserRoleUseCaseMapStore
    {
        private Dictionary<UserRole, List<string>> _map = null;

        public UserRoleUseCaseMapStore(Dictionary<UserRole, List<string>> map)
        {
            _map = map;
        }

        public List<string> GetUseCases(UserRole role)
        {
            if (_map is null || !_map.ContainsKey(role))
            {
                return new List<string>();
            }

            return _map[role];
        }
    }
}
