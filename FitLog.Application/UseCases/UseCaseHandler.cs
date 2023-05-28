using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Application.UseCases
{
    public abstract class UseCaseHandler<TUseCase, TData, TOut> : IUseCaseHandler where TUseCase : UseCase<TData, TOut>
    {
        public abstract TOut Handle(TUseCase useCase);
    }
}
