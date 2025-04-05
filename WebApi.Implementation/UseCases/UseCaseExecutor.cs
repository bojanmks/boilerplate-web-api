using System.Net;
using Newtonsoft.Json;
using WebApi.Application.ApplicationUsers;
using WebApi.Application.Logging;
using WebApi.Application.Logging.LoggerData;
using WebApi.Application.UseCases;
using WebApi.Application.Validation;
using WebApi.Common.DTO.Result;

namespace WebApi.Implementation.UseCases
{
    public class UseCaseExecutor<TUseCase, TData, TOut> where TUseCase : UseCase<TData, TOut>
    {
        private readonly IApplicationUser _applicationUser;
        private readonly IUseCaseLogger _useCaseLogger;
        private readonly IUseCaseSubscriberResolver _subscriberResolver;
        private readonly IValidatorResolver _validatorResolver;

        public UseCaseExecutor(IApplicationUser applicationUser, IUseCaseLogger useCaseLogger, IUseCaseSubscriberResolver subscriberResolver, IValidatorResolver validatorResolver)
        {
            _applicationUser = applicationUser;
            _useCaseLogger = useCaseLogger;
            _subscriberResolver = subscriberResolver;
            _validatorResolver = validatorResolver;
        }

        public async Task<Result<TOut>> Execute(TUseCase useCase, UseCaseHandler<TUseCase, TData, TOut> handler)
        {
            var validationResponse = await ValidateAndLog(useCase);

            if (!validationResponse.IsSuccess)
            {
                return validationResponse;
            }

            var response = await handler.HandleAsync(useCase);

            if (!response.IsSuccess)
            {
                return response;
            }

            await ExecuteUseCaseSubscribers(useCase, response.Data);

            return response;
        }

        private async Task<Result<TOut>> ValidateAndLog(TUseCase useCase)
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

            await _useCaseLogger.Log(log);

            if (!isAuthorized)
            {
                var errorResult = Result<TOut>.Error(Enumerable.Empty<string>());
                errorResult.HttpStatusCode = (int)HttpStatusCode.Forbidden;

                return errorResult;
            }

            var validator = _validatorResolver.Resolve<TUseCase>();

            if (validator is not null)
            {
                var validationResult = await validator.ValidateAsync(useCase);

                if (!validationResult.IsValid)
                {
                    var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage);

                    return Result<TOut>.ValidationError(errorMessages);
                }
            }

            return Result<TOut>.Success(default(TOut));
        }

        private async Task ExecuteUseCaseSubscribers(TUseCase useCase, TOut useCaseResponse)
        {
            var subscribers = _subscriberResolver.ResolveAll<TUseCase, TData, TOut>();

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
                await subscriber.OnUseCaseExecuted(subscriberData);
            }
        }
    }
}
