using WebApi.Application.Search;
using WebApi.Application.UseCases;
using WebApi.Common;
using WebApi.DataAccess.Entities.Abstraction;
using WebApi.Implementation.Core;
using WebApi.Implementation.UseCaseHandlers.Abstraction;

namespace WebApi.Implementation.UseCaseHandlers.Generic
{
    public class EfGenericSearchUseCaseHandler<TUseCase, TEntity, TOut> : EfUseCaseHandler<TUseCase, ISearchObject, object>
        where TUseCase : UseCase<ISearchObject, object>
        where TOut : IIdentifyable
        where TEntity : Entity
    {
        private readonly ISearchObjectQueryBuilder _searchObjectQueryBuilder;

        public EfGenericSearchUseCaseHandler(EntityAccessor accessor, ISearchObjectQueryBuilder searchObjectQueryBuilder) : base(accessor)
        {
            _searchObjectQueryBuilder = searchObjectQueryBuilder;
        }

        public override Task<object> HandleAsync(TUseCase useCase, CancellationToken cancellationToken = default)
        {
            var query = _accessor.GetQuery<TEntity>();

            var searchObj = useCase.Data;

            return _searchObjectQueryBuilder.BuildAndExecuteDynamicQueryAsync<TEntity, TOut>(searchObj, query);
        }
    }
}
