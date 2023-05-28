﻿using AutoMapper;
using FitLog.Application.Exceptions;
using FitLog.Application.UseCases;
using FitLog.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Implementation.UseCaseHandlers.Generic
{
    public class EfGenericFindUseCaseHandler<TUseCase, TEntity, TOut> : UseCaseHandler<TUseCase, int, TOut>
        where TUseCase : UseCase<int, TOut>
        where TEntity : class
    {
        private readonly FitLogContext _context;
        private readonly IMapper _mapper;

        public EfGenericFindUseCaseHandler(FitLogContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public override TOut Handle(TUseCase useCase)
        {
            var dataFromDb = _context.Set<TEntity>().Find(useCase.Data);

            if (dataFromDb is null)
            {
                throw new EntityNotFoundException();
            }

            return _mapper.Map<TOut>(dataFromDb);
        }
    }
}
