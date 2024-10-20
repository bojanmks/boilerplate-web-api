namespace WebApi.Application.UseCases
{
    public interface IUseCaseSubscriber<TUseCase, TData, TOut> where TUseCase : UseCase<TData, TOut>
    {
        void OnUseCaseExecuted(UseCaseSubscriberData<TData, TOut> data);
    }
}
