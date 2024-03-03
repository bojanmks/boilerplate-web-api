using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.UseCases.Users;
using WebApi.Common.DTO;
using WebApi.DataAccess.Entities;
using WebApi.Implementation.Search.SearchObjects;
using WebApi.Implementation.UseCases;

namespace WebApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly UseCaseMediator _mediator;

        public UsersController(UseCaseMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] UserSearch search)
        {
            return Ok(_mediator.Search<SearchUsersUseCase, User, UserDto>(new SearchUsersUseCase(search)));
        }
    }
}
