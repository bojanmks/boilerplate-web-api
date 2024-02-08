namespace WebApi.Application.UseCases
{
    public interface IUseCaseHandlerGetter
    {
        UseCaseHandler<TUseCase, TData, TOut> GetHandler<TUseCase, TData, TOut>() where TUseCase : UseCase<TData, TOut>;
    }
}
