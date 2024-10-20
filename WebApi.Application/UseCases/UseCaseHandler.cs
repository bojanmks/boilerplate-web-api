namespace WebApi.Application.UseCases
{
    public abstract class UseCaseHandler<TUseCase, TData, TOut> : IUseCaseHandler where TUseCase : UseCase<TData, TOut>
    {
        public abstract Task<TOut> HandleAsync(TUseCase useCase, CancellationToken cancellationToken = default);
    }
}
