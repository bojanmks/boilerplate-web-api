using AutoMapper;
using WebApi.Application.ApplicationUsers;
using WebApi.Application.Core;
using WebApi.Application.Logging;
using WebApi.Application.Search;
using WebApi.Application.UseCases;
using WebApi.Application.Validation;
using WebApi.Common.DTO.Abstraction;
using WebApi.DataAccess;
using WebApi.DataAccess.Entities.Abstraction;
using WebApi.Implementation.Exceptions;
using WebApi.Implementation.UseCaseHandlers.Generic;

namespace WebApi.Implementation.UseCases
{
    public class UseCaseMediator
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly IApplicationUser _applicationUser;
        private readonly IUseCaseLogger _useCaseLogger;
        private readonly IUseCaseSubscriberGetter _subscriberGetter;
        private readonly IValidatorGetter _validatorGetter;
        private readonly IUseCaseHandlerGetter _useCaseHandlerGetter;
        private readonly IEntityDeletionHandler _deleteHandler;
        private readonly ISearchObjectQueryBuilder _searchObjectQueryBuilder;

        public UseCaseMediator(DatabaseContext context,
                               IMapper mapper,
                               IApplicationUser applicationUser,
                               IUseCaseLogger useCaseLogger,
                               IUseCaseSubscriberGetter subscriberGetter,
                               IValidatorGetter validatorGetter,
                               IUseCaseHandlerGetter useCaseHandlerGetter,
                               IEntityDeletionHandler deleteHandler,
                               ISearchObjectQueryBuilder searchObjectQueryBuilder)
        {
            _context = context;
            _mapper = mapper;
            _applicationUser = applicationUser;
            _useCaseLogger = useCaseLogger;
            _subscriberGetter = subscriberGetter;
            _validatorGetter = validatorGetter;
            _useCaseHandlerGetter = useCaseHandlerGetter;
            _deleteHandler = deleteHandler;
            _searchObjectQueryBuilder = searchObjectQueryBuilder;
        }

        public object Search<TUseCase, TEntity, TOut>(TUseCase useCase)
            where TUseCase : UseCase<ISearchObject<TEntity>, object>
            where TOut : IIdentifyable
            where TEntity : Entity
        {
            var handler = new EfGenericSearchUseCaseHandler<TUseCase, TEntity, TOut>(_context, _searchObjectQueryBuilder);
            var executor = ConstructExecutor<TUseCase, ISearchObject<TEntity>, object>();

            return executor.Execute(useCase, handler);
        }

        public TOut Find<TUseCase, TEntity, TOut>(TUseCase useCase)
            where TUseCase : UseCase<int, TOut>
            where TEntity : Entity
        {
            var handler = new EfGenericFindUseCaseHandler<TUseCase, TEntity, TOut>(_context, _mapper);
            var executor = ConstructExecutor<TUseCase, int, TOut>();

            return executor.Execute(useCase, handler);
        }

        public Empty Insert<TUseCase, TData, TEntity>(TUseCase useCase)
            where TUseCase : UseCase<TData, Empty>
            where TEntity : Entity
        {
            var handler = new EfGenericInsertUseCaseHandler<TUseCase, TData, TEntity>(_context, _mapper);
            var executor = ConstructExecutor<TUseCase, TData, Empty>();

            return executor.Execute(useCase, handler);
        }

        public Empty Update<TUseCase, TData, TEntity>(TUseCase useCase)
            where TUseCase : UseCase<TData, Empty>
            where TData : IIdentifyable
            where TEntity : Entity
        {
            var handler = new EfGenericUpdateUseCaseHandler<TUseCase, TData, TEntity>(_context, _mapper);
            var executor = ConstructExecutor<TUseCase, TData, Empty>();

            return executor.Execute(useCase, handler);
        }

        public Empty Delete<TUseCase, TEntity>(TUseCase useCase)
            where TUseCase : UseCase<int, Empty>
            where TEntity : Entity
        {
            var handler = new EfGenericDeleteUseCaseHandler<TUseCase, TEntity>(_context, _deleteHandler);
            var executor = ConstructExecutor<TUseCase, int, Empty>();

            return executor.Execute(useCase, handler);
        }

        public TOut Execute<TUseCase, TData, TOut>(TUseCase useCase)
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
