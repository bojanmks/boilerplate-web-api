using AutoMapper;
using WebApi.Application.Exceptions;
using WebApi.Application.Localization;
using WebApi.Application.Search;
using WebApi.Common.Enums;
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

        public object BuildDynamicQuery<TEntity, TOut>(ISearchObject search, IQueryable<TEntity> query) where TEntity : Entity
        {
            query = BuildQuery(search, query);
            query = BuildOrderBy(search, query);

            var response = BuildResponse<TEntity, TOut>(search, query);

            return response;
        }

        public IQueryable<TEntity> BuildQuery<TEntity>(ISearchObject search, IQueryable<TEntity> query) where TEntity : Entity
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

        public IEnumerable<TOut> BuildQuery<TEntity, TOut>(ISearchObject search, IQueryable<TEntity> query) where TEntity : Entity
        {
            query = BuildQuery(search, query);

            return _mapper.Map<IEnumerable<TOut>>(query.ToList());
        }

        public IQueryable<TEntity> BuildOrderBy<TEntity>(ISearchObject search, IQueryable<TEntity> query) where TEntity : Entity
        {
            if (string.IsNullOrEmpty(search.SortBy) || search is not EfBaseSearch<TEntity>)
            {
                return query;
            }

            var sortByArgs = search.SortBy.Split(',');

            foreach (var arg in sortByArgs)
            {
                var propAndDirection = arg.Split('.');

                if (propAndDirection.Count() != 2)
                {
                    throw new InvalidSortFormatException(_translator);
                }

                string sortPropertyName = propAndDirection[0];
                SortDirection sortDirection;

                if (Enum.TryParse(typeof(SortDirection), propAndDirection[1], true, out var parsedSortDirection))
                {
                    sortDirection = (SortDirection)parsedSortDirection;
                }
                else
                {
                    throw new InvalidSortDirectionException(_translator);
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

            return query;
        }

        public IEnumerable<TOut> BuildOrderBy<TEntity, TOut>(ISearchObject search, IQueryable<TEntity> query) where TEntity : Entity
        {
            query = BuildOrderBy(search, query);

            return _mapper.Map<IEnumerable<TOut>>(query.ToList());
        }

        public object BuildResponse<TEntity, TOut>(ISearchObject search, IQueryable<TEntity> query) where TEntity : Entity
        {
            if (search.Paginate)
            {
                return new PagedResponse<TOut>
                {
                    Page = search.Page,
                    PerPage = search.PerPage,
                    TotalCount = query.Count(),
                    Items = _mapper.Map<IEnumerable<TOut>>(query.Skip((search.Page - 1) * search.PerPage).Take(search.PerPage).ToList())
                };
            }

            return _mapper.Map<IEnumerable<TOut>>(query.ToList());
        }
    }
}
