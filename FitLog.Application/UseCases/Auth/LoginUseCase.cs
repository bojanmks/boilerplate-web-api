using FitLog.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Application.UseCases.Auth
{
    public class LoginUseCase : UseCase<LoginData, Tokens>
    {
        public LoginUseCase(LoginData data) : base(data)
        {
        }

        public override string Id => "Login";
    }
}
