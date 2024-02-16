﻿using AutoMapper;
using WebApi.Application.UseCases;
using WebApi.DataAccess.Entities.Abstraction;
using WebApi.Implementation.Core;
using WebApi.Implementation.UseCaseHandlers.Abstraction;

namespace WebApi.Implementation.UseCaseHandlers.Generic
{
    internal class EfGenericInsertUseCaseHandler<TUseCase, TData, TEntity> : EfUseCaseHandler<TUseCase, TData, Empty>
        where TUseCase : UseCase<TData, Empty>
        where TEntity : Entity
    {
        private readonly IMapper _mapper;

        public EfGenericInsertUseCaseHandler(EntityAccessor accessor, IMapper mapper) : base(accessor)
        {
            _mapper = mapper;
        }

        public override Empty Handle(TUseCase useCase)
        {
            var entity = Activator.CreateInstance<TEntity>();

            _mapper.Map(useCase.Data, entity);

            _accessor.Add(entity);
            _accessor.SaveChanges();

            return Empty.Value;
        }
    }
}
