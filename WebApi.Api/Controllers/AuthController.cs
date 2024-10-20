using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.UseCases.Auth;
using WebApi.Common.DTO.Auth;
using WebApi.Implementation.UseCases;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Api.Controllers
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
