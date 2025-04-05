using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Localization;
using WebApi.Application.Search;
using WebApi.Common.DTO.Result;
using WebApi.Common.Enums.Filtering;
using WebApi.DataAccess.Entities.Abstraction;
using WebApi.Implementation.Extensions;

namespace WebApi.Implementation.Search
{
    public class EfSearchObjectQueryBuilder : ISearchObjectQueryBuilder
    {
        private readonly IMapper _mapper;
        private readonly ITranslator _translator;

        public EfSearchObjectQueryBuilder(IMapper mapper, ITranslator translator)
        {
            _mapper = mapper;
            _translator = translator;
        }

        public async Task<Result<object>> BuildAndExecuteDynamicQueryAsync<TEntity, TOut>(ISearchObject search, IQueryable<TEntity> query, CancellationToken cancellationToken = default) where TEntity : Entity
        {
            query = BuildQuery(search, query);

            var orderByResult = BuildOrderBy(search, query);

            if (!orderByResult.IsSuccess)
            {
                return Result<object>.Error(orderByResult.Errors);
            }

            query = orderByResult.Data;

            var result = await ExecuteSearchAsync<TEntity, TOut>(search, query, cancellationToken);

            return Result<object>.Success(result);
        }

        private IQueryable<TEntity> BuildQuery<TEntity>(ISearchObject search, IQueryable<TEntity> query) where TEntity : Entity
        {
            if (search is EfBaseSearch<TEntity> efSearch)
            {
                var filterExpressions = efSearch.GetFilterExpressions();

                foreach (var filterExpression in filterExpressions)
                {
                    query = query.Where(filterExpression);
                }
            }

            return query;
        }

        private Result<IQueryable<TEntity>> BuildOrderBy<TEntity>(ISearchObject search, IQueryable<TEntity> query) where TEntity : Entity
        {
            if (string.IsNullOrEmpty(search.SortBy) || search is not EfBaseSearch<TEntity>)
            {
                return Result<IQueryable<TEntity>>.Success(query);
            }

            var sortByArgs = search.SortBy.Split(',');

            foreach (var arg in sortByArgs)
            {
                var propAndDirection = arg.Split('.');

                if (propAndDirection.Count() != 2)
                {
                    return Result<IQueryable<TEntity>>.Error(new string[] { _translator.Translate("invalidSortStringFormat") });
                }

                string sortPropertyName = propAndDirection[0];
                SortDirection sortDirection;

                if (Enum.TryParse(typeof(SortDirection), propAndDirection[1], true, out var parsedSortDirection))
                {
                    sortDirection = (SortDirection)parsedSortDirection;
                }
                else
                {
                    return Result<IQueryable<TEntity>>.Error(new string[] { _translator.Translate("invalidSortDirection") });
                }

                var sortByExpression = ((EfBaseSearch<TEntity>)search).GetSortByPropertyExpression(sortPropertyName);

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

        private async Task<object> ExecuteSearchAsync<TEntity, TOut>(ISearchObject search, IQueryable<TEntity> query, CancellationToken cancellationToken = default) where TEntity : Entity
        {
            if (search.Paginate)
            {
                var items = await query.Skip((search.Page - 1) * search.PerPage).Take(search.PerPage).ToListAsync(cancellationToken);

                return new PagedResponse<TOut>
                {
                    Page = search.Page,
                    PerPage = search.PerPage,
                    TotalCount = query.Count(),
                    Items = _mapper.Map<IEnumerable<TOut>>(items)
                };
            }

            return _mapper.Map<IEnumerable<TOut>>(await query.ToListAsync(cancellationToken));
        }
    }
}
