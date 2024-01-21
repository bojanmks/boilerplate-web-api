using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Application.UseCases
{
    public abstract class UseCase<TData, TOut> : IUseCase
    {
        public TData Data { get; }

        public UseCase(TData data)
        {
            Data = data;
        }

        public abstract string Id { get; }
        public virtual string Description { get; }
    }
}
