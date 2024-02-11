using WebApi.Application.Search;
using WebApi.Application.UseCases;
using WebApi.Common.DTO.Abstraction;
using WebApi.DataAccess;

namespace WebApi.Implementation.UseCaseHandlers.Generic
{
    public class EfGenericSearchUseCaseHandler<TUseCase, TEntity, TOut> : UseCaseHandler<TUseCase, ISearchObject, object>
        where TUseCase : UseCase<ISearchObject, object>
        where TOut : IIdentifyable
        where TEntity : class
    {
        private readonly DatabaseContext _context;
        private readonly ISearchObjectQueryBuilder _searchObjectQueryBuilder;

        public EfGenericSearchUseCaseHandler(DatabaseContext context)
        {
            _context = context;
        }

        public override object Handle(TUseCase useCase)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            var searchObj = useCase.Data;

            return _searchObjectQueryBuilder.BuildDynamicQuery<TEntity, TOut>(searchObj, query);
        }
    }
}
