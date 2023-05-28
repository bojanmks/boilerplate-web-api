using FitLog.Application.Jwt;
using FitLog.Application.Localization;
using FitLog.Application.UseCases.Auth;
using FitLog.Common.DTO;
using FitLog.DataAccess;
using FitLog.Implementation.Exceptions;
using FitLog.Implementation.Extensions;
using FitLog.Implementation.UseCaseHandlers.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Implementation.UseCaseHandlers.Auth
{
    public class LoginUseCaseHandler : EfUseCaseHandler<LoginUseCase, LoginData, Tokens>
    {
        private readonly ITranslator _translator;
        private readonly IJwtTokenStorage _jwtTokenStorage;

        public LoginUseCaseHandler(FitLogContext context, ITranslator translator, IJwtTokenStorage jwtTokenStorage) : base(context)
        {
            _translator = translator;
            _jwtTokenStorage = jwtTokenStorage;
        }

        public override Tokens Handle(LoginUseCase useCase)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == useCase.Data.Email);

            if (user is null || !user.IsPasswordCorrect(useCase.Data.Password))
            {
                throw new TranslatableUnauthorizedAccessException(_translator, "invalidCredentials");
            }

            var tokens = _jwtTokenStorage.CreateRecord(user);

            return tokens;
        }
    }
}
