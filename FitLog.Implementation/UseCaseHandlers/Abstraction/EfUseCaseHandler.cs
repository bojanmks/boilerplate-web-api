using FitLog.Application.UseCases;
using FitLog.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Implementation.UseCaseHandlers.Abstraction
{
    public abstract class EfUseCaseHandler<TUseCase, TData, TOut> : UseCaseHandler<TUseCase, TData, TOut> where TUseCase : UseCase<TData, TOut>
    {
        protected readonly FitLogContext _context;
        public EfUseCaseHandler(FitLogContext context)
        {
            _context = context;
        }
    }
}
