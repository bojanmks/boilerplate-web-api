﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Application.UseCases
{
    public interface IUseCase
    {
        public string Id { get; }
        public string Description { get; }
    }
}
