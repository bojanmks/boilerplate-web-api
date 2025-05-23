﻿using AutoMapper;
using WebApi.Application.UseCases;
using WebApi.Common.DTO.Result;
using WebApi.DataAccess.Entities.Abstraction;
using WebApi.Implementation.Core;
using WebApi.Implementation.UseCaseHandlers.Abstraction;

namespace WebApi.Implementation.UseCaseHandlers.Generic
{
    public class EfGenericInsertUseCaseHandler<TUseCase, TData, TEntity> : EfUseCaseHandler<TUseCase, TData, Empty>
        where TUseCase : UseCase<TData, Empty>
        where TEntity : Entity
    {
        private readonly IMapper _mapper;

        public EfGenericInsertUseCaseHandler(EntityAccessor accessor, IMapper mapper) : base(accessor)
        {
            _mapper = mapper;
        }

        public override async Task<Result<Empty>> HandleAsync(TUseCase useCase, CancellationToken cancellationToken = default)
        {
            var entity = Activator.CreateInstance<TEntity>();

            _mapper.Map(useCase.Data, entity);

            await _accessor.AddAsync(entity, cancellationToken);
            await _accessor.SaveChangesAsync(cancellationToken);

            return Result<Empty>.Success(Empty.Value);
        }
    }
}
