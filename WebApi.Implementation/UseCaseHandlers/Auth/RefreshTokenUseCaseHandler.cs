using AutoMapper;
using WebApi.Application.Jwt;
using WebApi.Application.Localization;
using WebApi.Application.UseCases.Auth;
using WebApi.Common.DTO;
using WebApi.DataAccess;
using WebApi.Implementation.UseCaseHandlers.Abstraction;

namespace WebApi.Implementation.UseCaseHandlers.Auth
{
    public class RefreshTokenUseCaseHandler : EfUseCaseHandler<RefreshTokenUseCase, Tokens, Tokens>
    {
        private readonly IMapper _mapper;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IJwtTokenStorage _jwtTokenStorage;
        private readonly ITranslator _translator;

        public RefreshTokenUseCaseHandler(DatabaseContext context, IMapper mapper, IJwtTokenGenerator jwtTokenGenerator, IJwtTokenStorage jwtTokenStorage, ITranslator translator) : base(context)
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
