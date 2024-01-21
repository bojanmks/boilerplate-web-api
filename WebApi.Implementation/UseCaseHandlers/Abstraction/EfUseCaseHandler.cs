using WebApi.Application.UseCases;
using WebApi.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Implementation.UseCaseHandlers.Abstraction
{
    public abstract class EfUseCaseHandler<TUseCase, TData, TOut> : UseCaseHandler<TUseCase, TData, TOut> where TUseCase : UseCase<TData, TOut>
    {
        protected readonly DatabaseContext _context;
        public EfUseCaseHandler(DatabaseContext context)
        {
            _context = context;
        }
    }
}
