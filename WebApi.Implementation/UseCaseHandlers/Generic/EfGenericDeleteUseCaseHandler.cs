using WebApi.Application.UseCases;
using WebApi.DataAccess.Entities.Abstraction;
using WebApi.Implementation.Core;
using WebApi.Implementation.UseCaseHandlers.Abstraction;

namespace WebApi.Implementation.UseCaseHandlers.Generic
{
    public class EfGenericDeleteUseCaseHandler<TUseCase, TEntity> : EfUseCaseHandler<TUseCase, int, Empty>
        where TUseCase : UseCase<int, Empty>
        where TEntity : Entity
    {
        public EfGenericDeleteUseCaseHandler(EntityAccessor accessor) : base(accessor)
        {
        }

        public override Empty Handle(TUseCase useCase)
        {
            _accessor.Delete<TEntity>(useCase.Data);
            _accessor.SaveChanges();

            return Empty.Value;
        }
    }
}
