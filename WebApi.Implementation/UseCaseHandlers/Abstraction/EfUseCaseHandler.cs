using WebApi.Application.UseCases;
using WebApi.Implementation.Core;

namespace WebApi.Implementation.UseCaseHandlers.Abstraction
{
    public abstract class EfUseCaseHandler<TUseCase, TData, TOut> : UseCaseHandler<TUseCase, TData, TOut> where TUseCase : UseCase<TData, TOut>
    {
        protected readonly EntityAccessor _accessor;
        public EfUseCaseHandler(EntityAccessor accessor)
        {
            _accessor = accessor;
        }
    }
}
