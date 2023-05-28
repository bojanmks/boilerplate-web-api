using AutoMapper;
using FitLog.Application.Jwt;
using FitLog.Application.Localization;
using FitLog.Application.UseCases;
using FitLog.Application.UseCases.Auth;
using FitLog.Common.DTO;
using FitLog.DataAccess;
using FitLog.DataAccess.Entities;
using FitLog.Implementation.Jwt;
using FitLog.Implementation.UseCaseHandlers.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Implementation.UseCaseHandlers.Auth
{
    public class RefreshTokenUseCaseHandler : EfUseCaseHandler<RefreshTokenUseCase, Tokens, Tokens>
    {
        private readonly IMapper _mapper;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IJwtTokenStorage _jwtTokenStorage;
        private readonly ITranslator _translator;

        public RefreshTokenUseCaseHandler(FitLogContext context, IMapper mapper, IJwtTokenGenerator jwtTokenGenerator, IJwtTokenStorage jwtTokenStorage, ITranslator translator) : base(context)
        {
            _mapper = mapper;
            _jwtTokenGenerator = jwtTokenGenerator;
            _jwtTokenStorage = jwtTokenStorage;
            _translator = translator;
        }

        public override Tokens Handle(RefreshTokenUseCase useCase)
        {
            var tokenRecord = _jwtTokenStorage.FindByRefreshToken(useCase.Data.RefreshToken);

            var user = _context.Users.FirstOrDefault(x => x.Id == tokenRecord.UserId);

            if (user is null)
            {
                throw new UnauthorizedAccessException();
            }

            var tokens = _jwtTokenStorage.CreateRecord(user);

            _context.JwtTokenRecords.Remove(tokenRecord);
            _context.SaveChanges();

            return tokens;
        }
    }
}
