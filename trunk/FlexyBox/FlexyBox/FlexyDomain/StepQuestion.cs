using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexyDomain
{
    public class StepQuestion : EntityPersist
    {
        public string Header { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public int Order { get; set; }
    }
}
