using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Application.UseCases
{
    public class UseCaseSubscriberData<TData, TOut>
    {
        public TData UseCaseData { get; set; }
        public TOut Response { get; set; }
    }
}
