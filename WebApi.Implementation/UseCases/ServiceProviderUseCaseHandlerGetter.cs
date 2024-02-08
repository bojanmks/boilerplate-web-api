using Microsoft.Extensions.DependencyInjection;
using WebApi.Application.UseCases;

namespace WebApi.Implementation.UseCases
{
    public class ServiceProviderUseCaseHandlerGetter : IUseCaseHandlerGetter
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceProviderUseCaseHandlerGetter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public UseCaseHandler<TUseCase, TData, TOut> GetHandler<TUseCase, TData, TOut>() where TUseCase : UseCase<TData, TOut>
        {
            return _serviceProvider.GetService<UseCaseHandler<TUseCase, TData, TOut>>();
        }
    }
}
