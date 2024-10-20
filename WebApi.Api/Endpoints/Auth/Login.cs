using FastEndpoints;
using WebApi.Application.UseCases.Auth;
using WebApi.Common.DTO.Auth;
using WebApi.Implementation.UseCases;

namespace WebApi.Api.Endpoints.Auth;

public class Login(
    UseCaseMediator _mediator
) : Endpoint<LoginData, Tokens>
{
    public override void Configure()
    {
        Post("api/auth/Login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginData req, CancellationToken ct)
    {
        await SendAsync(_mediator.Execute<LoginUseCase, LoginData, Tokens>(new LoginUseCase(req)), cancellation: ct);
    }
}