using WebApi.Application.Search;
using WebApi.Application.UseCases;
using WebApi.Common.DTO.Abstraction;
using WebApi.DataAccess;
using WebApi.Implementation.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WebApi.Implementation.UseCaseHandlers.Generic
{
    public class EfGenericSearchUseCaseHandler<TUseCase, TEntity, TOut> : UseCaseHandler<TUseCase, ISearchObject, object>
        where TUseCase : UseCase<ISearchObject, object>
        where TOut : IIdentifyable
        where TEntity : class
    {
        private readonly DatabaseContext _context;

        public EfGenericSearchUseCaseHandler(DatabaseContext context)
        {
            _context = context;
        }

        public override object Handle(TUseCase useCase)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            var searchObj = useCase.Data;

            return searchObj.BuildDynamicQuery<TEntity, TOut>(query);
        }
    }
}
