using WebApi.Application.UseCases;
using WebApi.DataAccess;

namespace WebApi.Implementation.UseCaseHandlers.Abstraction
{
    public abstract class EfUseCaseHandler<TUseCase, TData, TOut> : UseCaseHandler<TUseCase, TData, TOut> where TUseCase : UseCase<TData, TOut>
    {
        protected readonly DatabaseContext _context;
        public EfUseCaseHandler(DatabaseContext context)
        {
            _context = context;
        }
    }
}
