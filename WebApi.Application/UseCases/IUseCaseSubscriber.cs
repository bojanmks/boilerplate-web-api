namespace WebApi.Application.UseCases
{
    public interface IUseCaseSubscriber<TUseCase, TData, TOut> where TUseCase : UseCase<TData, TOut>
    {
        public int Order { get; }
        void OnUseCaseExecuted(UseCaseSubscriberData<TData, TOut> data);
    }
}
