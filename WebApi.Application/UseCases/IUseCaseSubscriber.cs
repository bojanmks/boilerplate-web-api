namespace WebApi.Application.UseCases
{
    public interface IUseCaseSubscriber<TUseCase, TData, TOut> where TUseCase : UseCase<TData, TOut>
    {
        Task OnUseCaseExecuted(UseCaseSubscriberData<TData, TOut> data);
    }
}
