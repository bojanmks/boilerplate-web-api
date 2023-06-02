﻿using AutoMapper;
using FitLog.Application.ApplicationUsers;
using FitLog.Application.Logging;
using FitLog.Application.Search;
using FitLog.Application.UseCases;
using FitLog.Common.DTO.Abstraction;
using FitLog.DataAccess;
using FitLog.Implementation.Exceptions;
using FitLog.Implementation.UseCaseHandlers.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace FitLog.Implementation.UseCases
{
    public class UseCaseMediator
    {
        private readonly FitLogContext _context;
        private readonly IMapper _mapper;
        private readonly IServiceProvider _provider;
        public UseCaseMediator(FitLogContext context, 
                               IMapper mapper, 
                               IServiceProvider provider, 
                               IApplicationUser user, 
                               IUseCaseLogger logger,
                               UserRoleUseCaseMap userRoleUseCaseMap)
        {
            _context = context;
            _mapper = mapper;
            _provider = provider;
        }

        public object Search<TUseCase, TEntity, TOut>(TUseCase useCase)
            where TUseCase : UseCase<ISearchObject, object>
            where TOut : IIdentifyable
            where TEntity : class
        {
            var handler = new EfGenericSearchUseCaseHandler<TUseCase, TEntity, TOut>(_context);
            var executor = new UseCaseExecutor<TUseCase, ISearchObject, object>(_provider);

            return executor.Execute(useCase, handler);
        }

        public TOut Find<TUseCase, TEntity, TOut>(TUseCase useCase)
            where TUseCase : UseCase<int, TOut>
            where TEntity : class
        {
            var handler = new EfGenericFindUseCaseHandler<TUseCase, TEntity, TOut>(_context, _mapper);
            var executor = new UseCaseExecutor<TUseCase, int, TOut>(_provider);

            return executor.Execute(useCase, handler);
        }

        public Empty Insert<TUseCase, TData, TEntity>(TUseCase useCase)
            where TUseCase : UseCase<TData, Empty>
            where TEntity : class
        {
            var handler = new EfGenericInsertUseCaseHandler<TUseCase, TData, TEntity>(_context, _mapper);
            var executor = new UseCaseExecutor<TUseCase, TData, Empty>(_provider);

            return executor.Execute(useCase, handler);
        }

        public Empty Update<TUseCase, TData, TEntity>(TUseCase useCase)
            where TUseCase : UseCase<TData, Empty>
            where TData : IIdentifyable
            where TEntity : class
        {
            var handler = new EfGenericUpdateUseCaseHandler<TUseCase, TData, TEntity>(_context, _mapper);
            var executor = new UseCaseExecutor<TUseCase, TData, Empty>(_provider);

            return executor.Execute(useCase, handler);
        }

        public Empty Delete<TUseCase, TEntity>(TUseCase useCase)
            where TUseCase : UseCase<int, Empty>
            where TEntity : class
        {
            var handler = new EfGenericDeleteUseCaseHandler<TUseCase, TEntity>(_context);
            var executor = new UseCaseExecutor<TUseCase, int, Empty>(_provider);

            return executor.Execute(useCase, handler);
        }

        public object Execute<TUseCase, TData, TOut>(TUseCase useCase)
            where TUseCase : UseCase<TData, TOut>
        {
            var handler = _provider.GetService<UseCaseHandler<TUseCase, TData, TOut>>();

            if(handler is null)
            {
                throw new HandlerNotFoundException();
            }

            var executor = new UseCaseExecutor<TUseCase, TData, TOut>(_provider);

            return executor.Execute(useCase, handler);
        }
    }
}
