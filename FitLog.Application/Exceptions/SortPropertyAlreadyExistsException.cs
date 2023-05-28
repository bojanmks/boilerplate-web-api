using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Application.Exceptions
{
    public class SortPropertyAlreadyExistsException : Exception
    {
        public SortPropertyAlreadyExistsException(string key) : base($"Sort property with the key {key} already exists.")
        {

        }
    }
}
