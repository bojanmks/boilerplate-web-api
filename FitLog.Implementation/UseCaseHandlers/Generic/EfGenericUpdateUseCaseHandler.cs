using AutoMapper;
using FitLog.Application.Exceptions;
using FitLog.Application.UseCases;
using FitLog.Common.DTO.Abstraction;
using FitLog.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Implementation.UseCaseHandlers.Generic
{
    public class EfGenericUpdateUseCaseHandler<TUseCase, TData, TEntity> : UseCaseHandler<TUseCase, TData, Empty>
        where TUseCase : UseCase<TData, Empty>
        where TData : IIdentifyable
        where TEntity : class
    {
        private readonly FitLogContext _context;
        private readonly IMapper _mapper;

        public EfGenericUpdateUseCaseHandler(FitLogContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public override Empty Handle(TUseCase useCase)
        {
            var dataFromDb = _context.Set<TEntity>().Find(useCase.Data.Id);

            if (dataFromDb is null)
            {
                throw new EntityNotFoundException();
            }

            _mapper.Map(useCase.Data, dataFromDb);

            _context.SaveChanges();

            return Empty.Value;
        }
    }
}
