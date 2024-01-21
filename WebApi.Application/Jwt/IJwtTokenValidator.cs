using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Application.Jwt
{
    public interface IJwtTokenValidator
    {
        bool IsValid(string token);
    }
}
