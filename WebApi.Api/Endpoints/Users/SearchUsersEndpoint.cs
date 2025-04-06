using FastEndpoints;
using WebApi.Application.UseCases.Users;
using WebApi.Common.DTO.Users;
using WebApi.DataAccess.Entities;
using WebApi.Implementation.Search.SearchObjects;
using WebApi.Implementation.UseCases;

namespace WebApi.Api.Endpoints.Users;

public class SearchUsersEndpoint(
    UseCaseMediator _mediator
) : Endpoint<UserSearch, object>
{
    public override void Configure()
    {
        Get("api/users");
    }

    public override async Task HandleAsync(UserSearch req, CancellationToken ct)
    {
        var result = await _mediator.Search<SearchUsersUseCase, User, UserDto>(new SearchUsersUseCase(req));
        await SendAsync(result, cancellation: ct);
    }
}