using WebApi.Application.EntityDeletion;
using WebApi.Application.Exceptions;
using WebApi.Application.UseCases;
using WebApi.DataAccess;
using WebApi.DataAccess.Entities.Abstraction;

namespace WebApi.Implementation.UseCaseHandlers.Generic
{
    public class EfGenericDeleteUseCaseHandler<TUseCase, TEntity> : UseCaseHandler<TUseCase, int, Empty>
        where TUseCase : UseCase<int, Empty>
        where TEntity : Entity
    {
        private readonly DatabaseContext _context;
        private readonly IEntityDeletionHandler _deleteHandler;

        public EfGenericDeleteUseCaseHandler(DatabaseContext context, IEntityDeletionHandler deleteHandler)
        {
            _context = context;
            _deleteHandler = deleteHandler;
        }

        public override Empty Handle(TUseCase useCase)
        {
            var dataFromDb = _context.Set<TEntity>().Find(useCase.Data);

            if (dataFromDb is null)
            {
                throw new EntityNotFoundException();
            }

            _deleteHandler.Delete(dataFromDb);

            _context.SaveChanges();

            return Empty.Value;
        }
    }
}
