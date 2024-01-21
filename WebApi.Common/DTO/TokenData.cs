using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Common.DTO
{
    public class TokenData
    {
        public string Token { get; set; }
        public DateTime Expiry { get; set; }
    }
}
