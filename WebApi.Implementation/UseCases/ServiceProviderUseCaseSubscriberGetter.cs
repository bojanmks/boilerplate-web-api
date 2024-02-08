using Microsoft.Extensions.DependencyInjection;
using WebApi.Application.UseCases;

namespace WebApi.Implementation.UseCases
{
    public class ServiceProviderUseCaseSubscriberGetter : IUseCaseSubscriberGetter
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceProviderUseCaseSubscriberGetter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IEnumerable<IUseCaseSubscriber<TUseCase, TData, TOut>> GetSubscribers<TUseCase, TData, TOut>() where TUseCase : UseCase<TData, TOut>
        {
            return _serviceProvider.GetService<IEnumerable<IUseCaseSubscriber<TUseCase, TData, TOut>>>();
        }
    }
}
