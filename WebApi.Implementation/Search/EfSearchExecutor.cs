using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Search;
using WebApi.Common.DTO.Result;
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

        public async Task<Result<object>> ExecuteSearchAsync<TEntity, TOut>(ISearchObject search, IQueryable<TEntity> query, CancellationToken cancellationToken = default) where TEntity : Entity
        {
            object result;

            if (search.Paginate)
            {
                var items = await query.Skip((search.Page - 1) * search.PerPage).Take(search.PerPage).ToListAsync(cancellationToken);

                result = new PagedResponse<TOut>
                {
                    Page = search.Page,
                    PerPage = search.PerPage,
                    TotalCount = query.Count(),
                    Items = _mapper.Map<IEnumerable<TOut>>(items)
                };
            }

            result = _mapper.Map<IEnumerable<TOut>>(await query.ToListAsync(cancellationToken));

            return Result<object>.Success(result);
        }
    }
}
