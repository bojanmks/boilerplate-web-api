using WebApi.Application.Search;
using WebApi.Application.UseCases;
using WebApi.Common;
using WebApi.Common.DTO.Result;
using WebApi.DataAccess.Entities.Abstraction;
using WebApi.Implementation.Core;
using WebApi.Implementation.Search;
using WebApi.Implementation.UseCaseHandlers.Abstraction;

namespace WebApi.Implementation.UseCaseHandlers.Generic
{
    public class EfGenericSearchUseCaseHandler<TUseCase, TEntity, TOut> : EfUseCaseHandler<TUseCase, ISearchObject, SearchResult<TOut>>
        where TUseCase : UseCase<ISearchObject, SearchResult<TOut>>
        where TOut : IIdentifyable
        where TEntity : Entity
    {
        private readonly EfSearchObjectQueryBuilder _searchObjectQueryBuilder;
        private readonly EfSearchExecutor _searchExecutor;

        public EfGenericSearchUseCaseHandler(
            EntityAccessor accessor,
            EfSearchObjectQueryBuilder searchObjectQueryBuilder,
            EfSearchExecutor searchExecutor
        ) : base(accessor)
        {
            _searchObjectQueryBuilder = searchObjectQueryBuilder;
            _searchExecutor = searchExecutor;
        }

        public override async Task<Result<SearchResult<TOut>>> HandleAsync(TUseCase useCase, CancellationToken cancellationToken = default)
        {
            var query = _accessor.GetQuery<TEntity>();

            var searchObj = useCase.Data;

            if (searchObj is EfSearch<TEntity> efSearch)
            {
                var queryBuilderResult = _searchObjectQueryBuilder.BuildQuery<TEntity, TOut>(efSearch, query);

                if (!queryBuilderResult.IsSuccess)
                {
                    return queryBuilderResult.AsResultOfType<SearchResult<TOut>>();
                }

                query = queryBuilderResult.Data;
            }

            var result = await _searchExecutor.ExecuteSearchAsync<TEntity, TOut>(searchObj, query);

            return result;
        }
    }
}
