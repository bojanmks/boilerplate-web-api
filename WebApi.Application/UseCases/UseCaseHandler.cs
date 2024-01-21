namespace WebApi.Application.UseCases
{
    public abstract class UseCaseHandler<TUseCase, TData, TOut> : IUseCaseHandler where TUseCase : UseCase<TData, TOut>
    {
        public abstract TOut Handle(TUseCase useCase);
    }
}
