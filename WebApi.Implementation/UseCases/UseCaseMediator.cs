using AutoMapper;
using WebApi.Application.ApplicationUsers;
using WebApi.Application.Logging;
using WebApi.Application.Search;
using WebApi.Application.UseCases;
using WebApi.Application.Validation;
using WebApi.Common;
using WebApi.DataAccess.Entities.Abstraction;
using WebApi.Implementation.Core;
using WebApi.Implementation.Exceptions;
using WebApi.Implementation.UseCaseHandlers.Generic;

namespace WebApi.Implementation.UseCases
{
    public class UseCaseMediator
    {
        private readonly EntityAccessor _accessor;
        private readonly IMapper _mapper;
        private readonly IApplicationUser _applicationUser;
        private readonly IUseCaseLogger _useCaseLogger;
        private readonly IUseCaseSubscriberGetter _subscriberGetter;
        private readonly IValidatorGetter _validatorGetter;
        private readonly IUseCaseHandlerGetter _useCaseHandlerGetter;
        private readonly ISearchObjectQueryBuilder _searchObjectQueryBuilder;

        public UseCaseMediator(EntityAccessor accessor,
                               IMapper mapper,
                               IApplicationUser applicationUser,
                               IUseCaseLogger useCaseLogger,
                               IUseCaseSubscriberGetter subscriberGetter,
                               IValidatorGetter validatorGetter,
                               IUseCaseHandlerGetter useCaseHandlerGetter,
                               ISearchObjectQueryBuilder searchObjectQueryBuilder)
        {
            _accessor = accessor;
            _mapper = mapper;
            _applicationUser = applicationUser;
            _useCaseLogger = useCaseLogger;
            _subscriberGetter = subscriberGetter;
            _validatorGetter = validatorGetter;
            _useCaseHandlerGetter = useCaseHandlerGetter;
            _searchObjectQueryBuilder = searchObjectQueryBuilder;
        }

        public Task<object> Search<TUseCase, TEntity, TOut>(TUseCase useCase)
            where TUseCase : UseCase<ISearchObject, object>
            where TOut : IIdentifyable
            where TEntity : Entity
        {
            var handler = new EfGenericSearchUseCaseHandler<TUseCase, TEntity, TOut>(_accessor, _searchObjectQueryBuilder);
            var executor = ConstructExecutor<TUseCase, ISearchObject, object>();

            return executor.Execute(useCase, handler);
        }

        public Task<TOut> Find<TUseCase, TEntity, TOut>(TUseCase useCase)
            where TUseCase : UseCase<int, TOut>
            where TEntity : Entity
        {
            var handler = new EfGenericFindUseCaseHandler<TUseCase, TEntity, TOut>(_accessor, _mapper);
            var executor = ConstructExecutor<TUseCase, int, TOut>();

            return executor.Execute(useCase, handler);
        }

        public Task<Empty> Insert<TUseCase, TData, TEntity>(TUseCase useCase)
            where TUseCase : UseCase<TData, Empty>
            where TEntity : Entity
        {
            var handler = new EfGenericInsertUseCaseHandler<TUseCase, TData, TEntity>(_accessor, _mapper);
            var executor = ConstructExecutor<TUseCase, TData, Empty>();

            return executor.Execute(useCase, handler);
        }

        public Task<Empty> Update<TUseCase, TData, TEntity>(TUseCase useCase)
            where TUseCase : UseCase<TData, Empty>
            where TData : IIdentifyable
            where TEntity : Entity
        {
            var handler = new EfGenericUpdateUseCaseHandler<TUseCase, TData, TEntity>(_accessor, _mapper);
            var executor = ConstructExecutor<TUseCase, TData, Empty>();

            return executor.Execute(useCase, handler);
        }

        public Task<Empty> Delete<TUseCase, TEntity>(TUseCase useCase)
            where TUseCase : UseCase<int, Empty>
            where TEntity : Entity
        {
            var handler = new EfGenericDeleteUseCaseHandler<TUseCase, TEntity>(_accessor);
            var executor = ConstructExecutor<TUseCase, int, Empty>();

            return executor.Execute(useCase, handler);
        }

        public Task<TOut> Execute<TUseCase, TData, TOut>(TUseCase useCase)
            where TUseCase : UseCase<TData, TOut>
        {
            var handler = _useCaseHandlerGetter.GetHandler<TUseCase, TData, TOut>();

            if (handler is null)
            {
                throw new HandlerNotFoundException();
            }

            var executor = ConstructExecutor<TUseCase, TData, TOut>();

            return executor.Execute(useCase, handler);
        }

        private UseCaseExecutor<TUseCase, TData, TOut> ConstructExecutor<TUseCase, TData, TOut>() where TUseCase : UseCase<TData, TOut>
        {
            return new UseCaseExecutor<TUseCase, TData, TOut>(_applicationUser, _useCaseLogger, _subscriberGetter, _validatorGetter);
        }
    }
}
