using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.DataAccess.Entities.Abstraction
{
    public interface ISoftDeletable
    {
        public DateTime? DeletedAt { get; set; }
        public bool? IsActive { get; set; }
    }
}
