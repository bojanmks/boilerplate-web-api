using AutoMapper;
using WebApi.Application.Exceptions;
using WebApi.Application.UseCases;
using WebApi.DataAccess;
using WebApi.DataAccess.Entities.Abstraction;

namespace WebApi.Implementation.UseCaseHandlers.Generic
{
    public class EfGenericFindUseCaseHandler<TUseCase, TEntity, TOut> : UseCaseHandler<TUseCase, int, TOut>
        where TUseCase : UseCase<int, TOut>
        where TEntity : Entity
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public EfGenericFindUseCaseHandler(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public override TOut Handle(TUseCase useCase)
        {
            var dataFromDb = _context.Set<TEntity>().Find(useCase.Data);

            if (dataFromDb is null)
            {
                throw new EntityNotFoundException();
            }

            return _mapper.Map<TOut>(dataFromDb);
        }
    }
}
