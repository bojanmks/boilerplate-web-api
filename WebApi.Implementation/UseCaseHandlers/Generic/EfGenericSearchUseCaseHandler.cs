using WebApi.Application.Search;
using WebApi.Application.UseCases;
using WebApi.Common.DTO.Abstraction;
using WebApi.DataAccess;
using WebApi.DataAccess.Entities.Abstraction;

namespace WebApi.Implementation.UseCaseHandlers.Generic
{
    public class EfGenericSearchUseCaseHandler<TUseCase, TEntity, TOut> : UseCaseHandler<TUseCase, ISearchObject<TEntity>, object>
        where TUseCase : UseCase<ISearchObject<TEntity>, object>
        where TOut : IIdentifyable
        where TEntity : Entity
    {
        private readonly DatabaseContext _context;
        private readonly ISearchObjectQueryBuilder _searchObjectQueryBuilder;

        public EfGenericSearchUseCaseHandler(DatabaseContext context, ISearchObjectQueryBuilder searchObjectQueryBuilder)
        {
            _context = context;
            _searchObjectQueryBuilder = searchObjectQueryBuilder;
        }

        public override object Handle(TUseCase useCase)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            var searchObj = useCase.Data;

            return _searchObjectQueryBuilder.BuildDynamicQuery<TEntity, TOut>(searchObj, query);
        }
    }
}
