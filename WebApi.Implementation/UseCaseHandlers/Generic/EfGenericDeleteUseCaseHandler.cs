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

        public override async Task<Empty> HandleAsync(TUseCase useCase, CancellationToken cancellationToken = default)
        {
            await _accessor.DeleteByIdAsync<TEntity>(useCase.Data, cancellationToken: cancellationToken);
            await _accessor.SaveChangesAsync(cancellationToken);

            return Empty.Value;
        }
    }
}
