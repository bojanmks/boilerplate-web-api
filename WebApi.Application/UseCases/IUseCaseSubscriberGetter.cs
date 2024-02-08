namespace WebApi.Application.UseCases
{
    public interface IUseCaseSubscriberGetter
    {
        IEnumerable<IUseCaseSubscriber<TUseCase, TData, TOut>> GetSubscribers<TUseCase, TData, TOut>() where TUseCase : UseCase<TData, TOut>;
    }
}
