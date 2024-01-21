namespace WebApi.Application.UseCases
{
    public class UseCaseSubscriberData<TData, TOut>
    {
        public TData UseCaseData { get; set; }
        public TOut Response { get; set; }
    }
}
