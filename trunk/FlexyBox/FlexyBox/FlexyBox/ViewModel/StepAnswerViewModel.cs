using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexyBox.ViewModel
{
    public class StepAnswerViewModel
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public FlexyDomain.Models.EnumAnswer Answer { get; set; }
        public int EmployeeId { get; set; }
        public DateTime TimeChanged { get; set; }
        public bool IsLog { get; set; }
    }
}
