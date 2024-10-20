using FastEndpoints;
using WebApi.Application.UseCases.Auth;
using WebApi.Common.DTO.Auth;
using WebApi.Implementation.UseCases;

namespace WebApi.Api.Endpoints.Auth;

public class RefreshToken(
    UseCaseMediator _mediator
) : Endpoint<Tokens, Tokens>
{
    public override void Configure()
    {
        Post("api/auth/RefreshToken");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Tokens req, CancellationToken ct)
    {
        await SendAsync(_mediator.Execute<RefreshTokenUseCase, Tokens, Tokens>(new RefreshTokenUseCase(req)), cancellation: ct);
    }
}