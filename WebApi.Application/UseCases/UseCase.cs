namespace WebApi.Application.UseCases
{
    public abstract class UseCase<TData, TOut> : IUseCase<TData, TOut>
    {
        public TData Data { get; }

        protected UseCase(TData data)
        {
            Data = data;
        }

        public abstract string Id { get; }
    }
}
