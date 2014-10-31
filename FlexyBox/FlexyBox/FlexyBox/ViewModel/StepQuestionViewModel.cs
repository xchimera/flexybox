using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexyBox.ViewModel
{
    public class StepQuestionViewModel
    {
        public int Id { get; set; }
        public string Header{ get; set; }
        public string Description{ get; set; }
        public int Order { get; set; }
        public StepQuestionViewModel Child { get; set; }
        public StepQuestionViewModel Parent { get; set; }
        public StepAnswerViewModel Answer { get; set; }

    }
}
