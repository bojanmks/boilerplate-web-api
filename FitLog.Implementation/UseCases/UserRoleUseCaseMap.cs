using FitLog.Common.Attributes;
using FitLog.Common.Enums;
using FitLog.Implementation.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Implementation.UseCases
{
    public class UserRoleUseCaseMap
    {
        private Dictionary<UserRole, List<string>> Map => new Dictionary<UserRole, List<string>>
        {
            {
                UserRole.Admin,
                new List<string>
                {

                }
            },
            {
                UserRole.Regular,
                new List<string>
                {
                    "RefreshToken"
                }
            },
            {
                UserRole.Anonymous,
                new List<string>
                {
                    "Login", "RefreshToken"
                }
            }
        };

        public List<string> GetUseCases(UserRole userRole)
        {
            var useCases = GetInheritedUseCases(userRole);

            useCases.AddRange(Map[userRole]);

            return useCases.Distinct().ToList();
        }

        private List<string> GetInheritedUseCases(UserRole userRole)
        {
            var inheritedUseCases = new List<string>();
            var inheritUseCasesAttribute = userRole.GetAttributeOfType<InheritUseCasesAttribute>();

            if (inheritUseCasesAttribute is null)
            {
                return inheritedUseCases;
            }

            foreach (var role in inheritUseCasesAttribute.Roles)
            {
                inheritedUseCases.AddRange(Map[role]);
                inheritedUseCases.AddRange(GetInheritedUseCases(role));
            }

            return inheritedUseCases;
        }
    }
}
