using FitLog.Application.Search;
using FitLog.Application.UseCases;
using FitLog.Common.DTO.Abstraction;
using FitLog.DataAccess;
using FitLog.Implementation.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FitLog.Implementation.UseCaseHandlers.Generic
{
    public class EfGenericSearchUseCaseHandler<TUseCase, TEntity, TOut> : UseCaseHandler<TUseCase, ISearchObject, object>
        where TUseCase : UseCase<ISearchObject, object>
        where TOut : IIdentifyable
        where TEntity : class
    {
        private readonly FitLogContext _context;

        public EfGenericSearchUseCaseHandler(FitLogContext context)
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
