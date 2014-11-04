using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexyDomain.Models
{
    public class StepAnswer : EntityPersist
    {
        public string Comment { get; set; }
        public int EmployeeId { get; set; }
        public DateTime TimeChanged { get; set; }
        public bool IsLog { get; set; }
        public EnumAnswer QuestionAnswer { get; set; }
        public int QuestionId { get; set; }
        public bool IsDeleted { get; set; }
    }

    public enum EnumAnswer
    {
        Done = 1,
        NotDone = 2,
        NotApplicable = 3,
        NotAnswered = 0
    }
}
