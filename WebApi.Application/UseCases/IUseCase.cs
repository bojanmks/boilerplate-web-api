namespace WebApi.Application.UseCases
{
    public interface IUseCase<TData, TOut> : IUseCaseBase
    {
        TData Data { get; }
    }

    public interface IUseCaseBase
    {
        string Id { get; }
    }
}
