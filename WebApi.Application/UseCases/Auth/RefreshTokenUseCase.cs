using WebApi.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Application.UseCases.Auth
{
    public class RefreshTokenUseCase : UseCase<Tokens, Tokens>
    {
        public RefreshTokenUseCase(Tokens data) : base(data)
        {
        }

        public override string Id => "RefreshToken";
    }
}
