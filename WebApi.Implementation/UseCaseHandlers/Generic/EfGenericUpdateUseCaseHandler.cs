using AutoMapper;
using WebApi.Application.Exceptions;
using WebApi.Application.UseCases;
using WebApi.Common;
using WebApi.DataAccess.Entities.Abstraction;
using WebApi.Implementation.Core;
using WebApi.Implementation.UseCaseHandlers.Abstraction;

namespace WebApi.Implementation.UseCaseHandlers.Generic
{
    public class EfGenericUpdateUseCaseHandler<TUseCase, TData, TEntity> : EfUseCaseHandler<TUseCase, TData, Empty>
        where TUseCase : UseCase<TData, Empty>
        where TData : IIdentifyable
        where TEntity : Entity
    {
        private readonly IMapper _mapper;

        public EfGenericUpdateUseCaseHandler(EntityAccessor accessor, IMapper mapper) : base(accessor)
        {
            _mapper = mapper;
        }

        public override Empty Handle(TUseCase useCase)
        {
            var dataFromDb = _accessor.Find<TEntity>(useCase.Data.Id);

            if (dataFromDb is null)
            {
                throw new EntityNotFoundException();
            }

            _mapper.Map(useCase.Data, dataFromDb);

            _accessor.SaveChanges();

            return Empty.Value;
        }
    }
}
