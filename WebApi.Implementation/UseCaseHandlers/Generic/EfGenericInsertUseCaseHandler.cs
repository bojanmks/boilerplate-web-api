using AutoMapper;
using WebApi.Application.UseCases;
using WebApi.DataAccess;
using WebApi.DataAccess.Entities.Abstraction;

namespace WebApi.Implementation.UseCaseHandlers.Generic
{
    internal class EfGenericInsertUseCaseHandler<TUseCase, TData, TEntity> : UseCaseHandler<TUseCase, TData, Empty>
        where TUseCase : UseCase<TData, Empty>
        where TEntity : Entity
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public EfGenericInsertUseCaseHandler(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public override Empty Handle(TUseCase useCase)
        {
            var entity = Activator.CreateInstance<TEntity>();

            _mapper.Map(useCase.Data, entity);

            _context.Set<TEntity>().Add(entity);

            _context.SaveChanges();

            return Empty.Value;
        }
    }
}
