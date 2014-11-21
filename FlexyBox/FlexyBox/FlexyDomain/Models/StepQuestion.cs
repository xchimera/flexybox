using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexyDomain.Models
{
    public class StepQuestion : EntityPersist
    {
        public string Header { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public int Order { get; set; }
        public StepQuestion Parent { get; set; }
        public IList<StepQuestion> Children { get; set; }
        public Product Product { get; set; }

        //public StepQuestion ChildQuestion { get; set; }
        public int GroupId { get; set; }
    }
}
