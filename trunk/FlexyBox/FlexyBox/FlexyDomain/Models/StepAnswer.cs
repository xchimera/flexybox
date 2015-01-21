using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexyDomain.Models
{
    //Lavet af Vijeeth
    public class StepAnswer : EntityPersist
    {       
        public int QuestionId { get; set; }
        public CustomerFlow CustomerFlow { get; set; }
        public string Comment { get; set; }
        public int EmployeeId { get; set; }
        public DateTime TimeChanged { get; set; }
        public bool IsLog { get; set; }
        public AnswerState State { get; set; }
    }

    public enum AnswerState
    {
        Done = 1,
        NotDone = 2,
        NotApplicable = 3,
        NotAnswered = 0
    }
}
