using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using WebApi.Application.ApplicationUsers;
using WebApi.Application.Logging;
using WebApi.Application.Logging.LoggerData;
using WebApi.Application.UseCases;
using WebApi.Implementation.Exceptions;

namespace WebApi.Implementation.UseCases
{
    public class UseCaseExecutor<TUseCase, TData, TOut> where TUseCase : UseCase<TData, TOut>
    {
        private readonly IServiceProvider _provider;

        public UseCaseExecutor(IServiceProvider provider)
        {
            _provider = provider;
        }

        public TOut Execute(TUseCase useCase, UseCaseHandler<TUseCase, TData, TOut> handler)
        {
            ValidateAndLog(useCase);

            var response = handler.Handle(useCase);

            ExecuteUseCaseSubscribers(useCase, response);

            return response;
        }

        private void ValidateAndLog(TUseCase useCase)
        {
            var user = _provider.GetService<IApplicationUser>();
            var useCaseLogger = _provider.GetService<IUseCaseLogger>();

            var isAuthorized = user.AllowedUseCases.Contains(useCase.Id);

            var log = new UseCaseLoggerData
            {
                UserId = user.Id,
                UseCaseId = useCase.Id,
                IsAuthorized = isAuthorized,
                ExecutionDateTime = DateTime.UtcNow,
                Data = JsonConvert.SerializeObject(useCase.Data)
            };

            useCaseLogger.Log(log);

            if (!isAuthorized)
            {
                throw new ForbiddenUseCaseException(useCase.Id, user.Id.ToString());
            }

            var validator = _provider.GetService<AbstractValidator<TUseCase>>();

            if (validator is not null)
            {
                validator.ValidateAndThrow(useCase);
            }
        }

        private void ExecuteUseCaseSubscribers(TUseCase useCase, TOut useCaseResponse)
        {
            var subscribers = _provider.GetService<IEnumerable<IUseCaseSubscriber<TUseCase, TData, TOut>>>();

            if (subscribers is null)
            {
                return;
            }

            foreach (var subscriber in subscribers.OrderBy(x => x.Order))
            {
                subscriber.OnUseCaseExecuted(new UseCaseSubscriberData<TData, TOut>
                {
                    UseCaseData = useCase.Data,
                    Response = useCaseResponse
                });
            }
        }
    }
}
