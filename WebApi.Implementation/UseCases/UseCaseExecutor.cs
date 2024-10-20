﻿using FluentValidation;
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
        private readonly IUseCaseSubscriberResolver _subscriberGetter;
        private readonly IValidatorResolver _validatorGetter;

        public UseCaseExecutor(IApplicationUser applicationUser, IUseCaseLogger useCaseLogger, IUseCaseSubscriberResolver subscriberGetter, IValidatorResolver validatorGetter)
        {
            _applicationUser = applicationUser;
            _useCaseLogger = useCaseLogger;
            _subscriberGetter = subscriberGetter;
            _validatorGetter = validatorGetter;
        }

        public async Task<TOut> Execute(TUseCase useCase, UseCaseHandler<TUseCase, TData, TOut> handler)
        {
            await ValidateAndLog(useCase);

            var response = await handler.HandleAsync(useCase);

            await ExecuteUseCaseSubscribers(useCase, response);

            return response;
        }

        private async Task ValidateAndLog(TUseCase useCase)
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
                throw new ForbiddenUseCaseException(useCase.Id, _applicationUser.Id.ToString());
            }

            var validator = _validatorGetter.Resolve<TUseCase>();

            if (validator is not null)
            {
                await validator.ValidateAndThrowAsync(useCase);
            }
        }

        private async Task ExecuteUseCaseSubscribers(TUseCase useCase, TOut useCaseResponse)
        {
            var subscribers = _subscriberGetter.ResolveAll<TUseCase, TData, TOut>();

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
