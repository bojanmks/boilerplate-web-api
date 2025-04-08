using AutoMapper;
using WebApi.Application.ApplicationUsers;
using WebApi.Application.Logging;
using WebApi.Application.Search;
using WebApi.Application.UseCases;
using WebApi.Application.Validation;
using WebApi.Common;
using WebApi.Common.DTO.Result;
using WebApi.DataAccess.Entities.Abstraction;
using WebApi.Implementation.Core;
using WebApi.Implementation.Exceptions;
using WebApi.Implementation.Search;
using WebApi.Implementation.UseCaseHandlers.Generic;

namespace WebApi.Implementation.UseCases
{
    public class UseCaseMediator
    {
        private readonly EntityAccessor _accessor;
        private readonly IMapper _mapper;
        private readonly IApplicationUser _applicationUser;
        private readonly IUseCaseLogger _useCaseLogger;
        private readonly IUseCaseSubscriberResolver _subscriberResolver;
        private readonly IValidatorResolver _validatorResolver;
        private readonly IUseCaseHandlerResolver _useCaseHandlerResolver;
        private readonly EfSearchObjectQueryBuilder _searchObjectQueryBuilder;
        private readonly EfSearchExecutor _searchExecutor;

        public UseCaseMediator(
            EntityAccessor accessor,
            IMapper mapper,
            IApplicationUser applicationUser,
            IUseCaseLogger useCaseLogger,
            IUseCaseSubscriberResolver subscriberResolver,
            IValidatorResolver validatorResolver,
            IUseCaseHandlerResolver useCaseHandlerResolver,
            EfSearchObjectQueryBuilder searchObjectQueryBuilder,
            EfSearchExecutor searchExecutor
        )
        {
            _accessor = accessor;
            _mapper = mapper;
            _applicationUser = applicationUser;
            _useCaseLogger = useCaseLogger;
            _subscriberResolver = subscriberResolver;
            _validatorResolver = validatorResolver;
            _useCaseHandlerResolver = useCaseHandlerResolver;
            _searchObjectQueryBuilder = searchObjectQueryBuilder;
            _searchExecutor = searchExecutor;
        }

        public Task<Result<SearchResult<TOut>>> Search<TUseCase, TEntity, TOut>(TUseCase useCase)
            where TUseCase : UseCase<ISearchObject, SearchResult<TOut>>
            where TOut : IIdentifyable
            where TEntity : Entity
        {
            var handler = new EfGenericSearchUseCaseHandler<TUseCase, TEntity, TOut>(_accessor, _searchObjectQueryBuilder, _searchExecutor);
            var executor = ConstructExecutor<TUseCase, ISearchObject, SearchResult<TOut>>();

            return executor.Execute(useCase, handler);
        }

        public Task<Result<TOut>> Find<TUseCase, TEntity, TOut>(TUseCase useCase)
            where TUseCase : UseCase<int, TOut>
            where TEntity : Entity
        {
            var handler = new EfGenericFindUseCaseHandler<TUseCase, TEntity, TOut>(_accessor, _mapper);
            var executor = ConstructExecutor<TUseCase, int, TOut>();

            return executor.Execute(useCase, handler);
        }

        public Task<Result<Empty>> Insert<TUseCase, TData, TEntity>(TUseCase useCase)
            where TUseCase : UseCase<TData, Empty>
            where TEntity : Entity
        {
            var handler = new EfGenericInsertUseCaseHandler<TUseCase, TData, TEntity>(_accessor, _mapper);
            var executor = ConstructExecutor<TUseCase, TData, Empty>();

            return executor.Execute(useCase, handler);
        }

        public Task<Result<Empty>> Update<TUseCase, TData, TEntity>(TUseCase useCase)
            where TUseCase : UseCase<TData, Empty>
            where TData : IIdentifyable
            where TEntity : Entity
        {
            var handler = new EfGenericUpdateUseCaseHandler<TUseCase, TData, TEntity>(_accessor, _mapper);
            var executor = ConstructExecutor<TUseCase, TData, Empty>();

            return executor.Execute(useCase, handler);
        }

        public Task<Result<Empty>> Delete<TUseCase, TEntity>(TUseCase useCase)
            where TUseCase : UseCase<int, Empty>
            where TEntity : Entity
        {
            var handler = new EfGenericDeleteUseCaseHandler<TUseCase, TEntity>(_accessor);
            var executor = ConstructExecutor<TUseCase, int, Empty>();

            return executor.Execute(useCase, handler);
        }

        public Task<Result<TOut>> Execute<TUseCase, TData, TOut>(TUseCase useCase)
            where TUseCase : UseCase<TData, TOut>
        {
            var handler = _useCaseHandlerResolver.Resolve<TUseCase, TData, TOut>();

            if (handler is null)
            {
                throw new HandlerNotFoundException();
            }

            var executor = ConstructExecutor<TUseCase, TData, TOut>();

            return executor.Execute(useCase, handler);
        }

        private UseCaseExecutor<TUseCase, TData, TOut> ConstructExecutor<TUseCase, TData, TOut>() where TUseCase : UseCase<TData, TOut>
        {
            return new UseCaseExecutor<TUseCase, TData, TOut>(_applicationUser, _useCaseLogger, _subscriberResolver, _validatorResolver);
        }
    }
}
