using AutoMapper;
using WebApi.Application.Exceptions;
using WebApi.Application.UseCases;
using WebApi.Common.DTO.Abstraction;
using WebApi.DataAccess;

namespace WebApi.Implementation.UseCaseHandlers.Generic
{
    public class EfGenericUpdateUseCaseHandler<TUseCase, TData, TEntity> : UseCaseHandler<TUseCase, TData, Empty>
        where TUseCase : UseCase<TData, Empty>
        where TData : IIdentifyable
        where TEntity : class
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public EfGenericUpdateUseCaseHandler(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public override Empty Handle(TUseCase useCase)
        {
            var dataFromDb = _context.Set<TEntity>().Find(useCase.Data.Id);

            if (dataFromDb is null)
            {
                throw new EntityNotFoundException();
            }

            _mapper.Map(useCase.Data, dataFromDb);

            _context.SaveChanges();

            return Empty.Value;
        }
    }
}
