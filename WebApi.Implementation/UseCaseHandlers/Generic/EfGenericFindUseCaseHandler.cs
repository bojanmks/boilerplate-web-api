using AutoMapper;
using WebApi.Application.Exceptions;
using WebApi.Application.UseCases;
using WebApi.DataAccess.Entities.Abstraction;
using WebApi.Implementation.Core;
using WebApi.Implementation.UseCaseHandlers.Abstraction;

namespace WebApi.Implementation.UseCaseHandlers.Generic
{
    public class EfGenericFindUseCaseHandler<TUseCase, TEntity, TOut> : EfUseCaseHandler<TUseCase, int, TOut>
        where TUseCase : UseCase<int, TOut>
        where TEntity : Entity
    {
        private readonly IMapper _mapper;

        public EfGenericFindUseCaseHandler(EntityAccessor accessor, IMapper mapper) : base(accessor)
        {
            _mapper = mapper;
        }

        public override async Task<TOut> HandleAsync(TUseCase useCase, CancellationToken cancellationToken = default)
        {
            var dataFromDb = await _accessor.FindByIdAsync<TEntity>(useCase.Data, cancellationToken: cancellationToken);

            if (dataFromDb is null)
            {
                throw new EntityNotFoundException();
            }

            return _mapper.Map<TOut>(dataFromDb);
        }
    }
}
