using FitLog.Application.UseCases.Auth;
using FitLog.Common.DTO;
using FitLog.Implementation.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FitLog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UseCaseMediator _mediator;

        public AuthController(UseCaseMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginData data)
        {
            return Ok(_mediator.Execute<LoginUseCase, LoginData, Tokens>(new LoginUseCase(data)));
        }

        [HttpPost("RefreshToken")]
        [AllowAnonymous]
        public IActionResult RefreshToken([FromBody] Tokens data)
        {
            return Ok(_mediator.Execute<RefreshTokenUseCase, Tokens, Tokens>(new RefreshTokenUseCase(data)));
        }
    }
}
