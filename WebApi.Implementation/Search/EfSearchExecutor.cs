using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Search;
using WebApi.Common.DTO.Result;
using WebApi.Common.DTO.Search;
using WebApi.DataAccess.Entities.Abstraction;

namespace WebApi.Implementation.Search
{
    public class EfSearchExecutor
    {
        private readonly IMapper _mapper;

        public EfSearchExecutor(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<Result<SearchResult<TOut>>> ExecuteSearchAsync<TEntity, TOut>(ISearchObject search, IQueryable<TEntity> query, CancellationToken cancellationToken = default)
            where TEntity : Entity
        {
            SearchResult<TOut> result;

            if (search.Paginate)
            {
                var items = await query.Skip((search.Page - 1) * search.PerPage).Take(search.PerPage).ToListAsync(cancellationToken);

                result = new SearchResult<TOut>
                {
                    Items = _mapper.Map<IEnumerable<TOut>>(items),
                    PaginationSettings = new PaginationSettings
                    {
                        Page = search.Page,
                        PerPage = search.PerPage,
                        TotalCount = query.Count()
                    }
                };
            }
            else
            {
                result = new SearchResult<TOut>
                {
                    Items = _mapper.Map<IEnumerable<TOut>>(await query.ToListAsync(cancellationToken))
                };
            }

            return Result<SearchResult<TOut>>.Success(result);
        }
    }
}
