using WebApi.Application.Exceptions;
using WebApi.Application.UseCases;
using WebApi.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Implementation.UseCaseHandlers.Generic
{
    public class EfGenericDeleteUseCaseHandler<TUseCase, TEntity> : UseCaseHandler<TUseCase, int, Empty>
        where TUseCase : UseCase<int, Empty>
        where TEntity : class
    {
        private readonly DatabaseContext _context;

        public EfGenericDeleteUseCaseHandler(DatabaseContext context)
        {
            _context = context;
        }

        public override Empty Handle(TUseCase useCase)
        {
            var dataFromDb = _context.Set<TEntity>().Find(useCase.Data);

            if (dataFromDb is null)
            {
                throw new EntityNotFoundException();
            }

            _context.Set<TEntity>().Remove(dataFromDb);

            _context.SaveChanges();

            return Empty.Value;
        }
    }
}
