using AutoMapper;
using FitLog.Application.UseCases;
using FitLog.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Implementation.UseCaseHandlers.Generic
{
    internal class EfGenericInsertUseCaseHandler<TUseCase, TData, TEntity> : UseCaseHandler<TUseCase, TData, Empty>
        where TUseCase : UseCase<TData, Empty>
        where TEntity : class
    {
        private readonly FitLogContext _context;
        private readonly IMapper _mapper;

        public EfGenericInsertUseCaseHandler(FitLogContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public override Empty Handle(TUseCase useCase)
        {
            var mappedData = _mapper.Map<TData, TEntity>(useCase.Data);

            _context.Set<TEntity>().Add(mappedData);

            _context.SaveChanges();

            return Empty.Value;
        }
    }
}
