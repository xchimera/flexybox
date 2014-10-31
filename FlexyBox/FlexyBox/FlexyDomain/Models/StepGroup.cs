using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexyDomain.Models
{
    public class StepGroup : EntityPersist
    {
        public string Header { get; set; }
        public IList<StepQuestion> Questions { get; set; }
    }
}
