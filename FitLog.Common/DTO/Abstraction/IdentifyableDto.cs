﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Common.DTO.Abstraction
{
    public abstract class IdentifyableDto : IIdentifyable
    {
        public int Id { get; set; }
    }
}
