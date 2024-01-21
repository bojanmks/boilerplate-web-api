using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Application.UseCases
{
    public interface IUseCaseSubscriber<TUseCase, TData, TOut> where TUseCase : UseCase<TData, TOut>
    {
        public int Order { get; }
        void OnUseCaseExecuted(UseCaseSubscriberData<TData, TOut> data);
    }
}
