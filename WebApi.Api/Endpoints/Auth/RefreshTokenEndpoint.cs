using WebApi.Application.UseCases.Auth;
using WebApi.Common.DTO.Auth;
using WebApi.Implementation.UseCases;

namespace WebApi.Api.Endpoints.Auth;

public class RefreshTokenEndpoint(
    UseCaseMediator _mediator
) : BaseEndpoint<Tokens, Tokens>
{
    public override void Configure()
    {
        Post("api/auth/RefreshToken");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Tokens req, CancellationToken ct)
    {
        var result = await _mediator.Execute<RefreshTokenUseCase, Tokens, Tokens>(new RefreshTokenUseCase(req));
        await RespondFromResult(result, ct);
    }
}