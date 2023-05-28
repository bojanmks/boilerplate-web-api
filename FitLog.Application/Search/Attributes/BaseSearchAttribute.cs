using FitLog.Application.Search.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Application.Search.Attributes
{
    public abstract class BaseSearchAttribute : Attribute
    {
        private readonly ComparisonType _comparisonType;

        public BaseSearchAttribute(ComparisonType comparisonType)
        {
            _comparisonType = comparisonType;
        }

        public ComparisonType ComparisonType => _comparisonType;
    }
}
