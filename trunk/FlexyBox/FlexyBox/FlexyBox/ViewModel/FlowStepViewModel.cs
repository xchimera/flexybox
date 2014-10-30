using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexyBox.ViewModel
{
    public class FlowStepViewModel
    {
        public string Header{ get; set; }
        public string Description{ get; set; }
        public int Order { get; set; }
        public FlowStepViewModel Child { get; set; }
        public FlowStepViewModel Parent { get; set; }
    }
}
