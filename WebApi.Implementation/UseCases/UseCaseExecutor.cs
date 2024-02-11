using FluentValidation;
using Newtonsoft.Json;
using WebApi.Application.ApplicationUsers;
using WebApi.Application.Logging;
using WebApi.Application.Logging.LoggerData;
using WebApi.Application.UseCases;
using WebApi.Application.Validation;
using WebApi.Implementation.Exceptions;

namespace WebApi.Implementation.UseCases
{
    public class UseCaseExecutor<TUseCase, TData, TOut> where TUseCase : UseCase<TData, TOut>
    {
        private readonly IApplicationUser _applicationUser;
        private readonly IUseCaseLogger _useCaseLogger;
        private readonly IUseCaseSubscriberGetter _subscriberGetter;
        private readonly IValidatorGetter _validatorGetter;

        public UseCaseExecutor(IApplicationUser applicationUser, IUseCaseLogger useCaseLogger, IUseCaseSubscriberGetter subscriberGetter, IValidatorGetter validatorGetter)
        {
            _applicationUser = applicationUser;
            _useCaseLogger = useCaseLogger;
            _subscriberGetter = subscriberGetter;
            _validatorGetter = validatorGetter;
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
            var isAuthorized = _applicationUser.AllowedUseCases.Contains(useCase.Id);

            var log = new UseCaseLoggerData
            {
                UserId = _applicationUser.Id,
                UseCaseId = useCase.Id,
                IsAuthorized = isAuthorized,
                ExecutionDateTime = DateTime.UtcNow,
                Data = JsonConvert.SerializeObject(useCase.Data)
            };

            _useCaseLogger.Log(log);

            if (!isAuthorized)
            {
                throw new ForbiddenUseCaseException(useCase.Id, _applicationUser.Id.ToString());
            }

            var validator = _validatorGetter.GetValidator<TUseCase>();

            if (validator is not null)
            {
                validator.ValidateAndThrow(useCase);
            }
        }

        private void ExecuteUseCaseSubscribers(TUseCase useCase, TOut useCaseResponse)
        {
            var subscribers = _subscriberGetter.GetSubscribers<TUseCase, TData, TOut>();

            if (subscribers is null)
            {
                return;
            }

            var subscriberData = new UseCaseSubscriberData<TData, TOut>
            {
                UseCaseData = useCase.Data,
                Response = useCaseResponse
            };

            foreach (var subscriber in subscribers)
            {
                subscriber.OnUseCaseExecuted(subscriberData);
            }
        }
    }
}
