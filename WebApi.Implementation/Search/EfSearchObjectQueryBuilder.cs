using WebApi.Application.Localization;
using WebApi.Application.Search;
using WebApi.Common.DTO.Result;
using WebApi.Common.Enums.Filtering;
using WebApi.DataAccess.Entities.Abstraction;
using WebApi.Implementation.Extensions;

namespace WebApi.Implementation.Search
{
    public class EfSearchObjectQueryBuilder
    {
        private readonly ITranslator _translator;

        public EfSearchObjectQueryBuilder(ITranslator translator)
        {
            _translator = translator;
        }

        public Result<IQueryable<TEntity>> BuildQuery<TEntity, TOut>(EfSearch<TEntity> search, IQueryable<TEntity> query, CancellationToken cancellationToken = default) where TEntity : Entity
        {
            query = BuildWhere(search, query);

            var orderByResult = BuildOrderBy(search, query);

            if (!orderByResult.IsSuccess)
            {
                return orderByResult;
            }

            query = orderByResult.Data;

            return Result<IQueryable<TEntity>>.Success(query);
        }

        private IQueryable<TEntity> BuildWhere<TEntity>(EfSearch<TEntity> search, IQueryable<TEntity> query) where TEntity : Entity
        {
            var filterExpressions = search.GetFilterExpressions();

            foreach (var filterExpression in filterExpressions)
            {
                query = query.Where(filterExpression);
            }

            return query;
        }

        private Result<IQueryable<TEntity>> BuildOrderBy<TEntity>(EfSearch<TEntity> search, IQueryable<TEntity> query) where TEntity : Entity
        {
            if (string.IsNullOrEmpty(search.SortBy))
            {
                return Result<IQueryable<TEntity>>.Success(query);
            }

            var sortByArgs = search.SortBy.Split(',');

            foreach (var arg in sortByArgs)
            {
                var propAndDirection = arg.Split('.');

                if (propAndDirection.Count() != 2)
                {
                    return Result<IQueryable<TEntity>>.Error([_translator.Translate("invalidSortStringFormat")]);
                }

                string sortPropertyName = propAndDirection[0];
                SortDirection sortDirection;

                if (Enum.TryParse(typeof(SortDirection), propAndDirection[1], true, out var parsedSortDirection))
                {
                    sortDirection = (SortDirection)parsedSortDirection;
                }
                else
                {
                    return Result<IQueryable<TEntity>>.Error([_translator.Translate("invalidSortDirection")]);
                }

                var sortByExpression = search.GetSortByPropertyExpression(sortPropertyName);

                if (sortByExpression is null)
                {
                    continue;
                }

                switch (sortDirection)
                {
                    case SortDirection.Asc:
                        query = query.IsOrdered()
                            ? ((IOrderedQueryable<TEntity>)query).ThenBy(sortByExpression)
                            : query.OrderBy(sortByExpression);
                        break;

                    case SortDirection.Desc:
                        query = query.IsOrdered()
                            ? ((IOrderedQueryable<TEntity>)query).ThenByDescending(sortByExpression)
                            : query.OrderByDescending(sortByExpression);
                        break;
                }
            }

            return Result<IQueryable<TEntity>>.Success(query);
        }
    }
}
