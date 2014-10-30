using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexyDomain.Models
{
    class CustomerFlow : EntityPersist
    {
        public int CustomerId { get; set; }
        IList<StepAnswer> Answers { get; set; }
    }
}
