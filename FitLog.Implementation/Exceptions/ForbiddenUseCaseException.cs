using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Implementation.Exceptions
{
    public class ForbiddenUseCaseException : Exception
    {
        public ForbiddenUseCaseException(string useCaseId, string userId) : base($"User with an Id of {userId} does not have permission to execute the {useCaseId} use case.")
        {
            
        }
    }
}
