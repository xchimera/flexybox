using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlexyDomain.Models
{
    //Lavet af Vijeeth
    public class QuestionVisibility : EntityPersist
    {
        public StepQuestion Question { get; set; }
        public IList<StepQuestion> Questions { get; set; }
    }
}
