using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.DataAccess.Entities.Abstraction
{
    public abstract class Entity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
