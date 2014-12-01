using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexyDomain.Models
{
    public class CustomerFile : EntityPersist
    {
        public string Name { get; set; }
        public virtual byte[] File { get; set; }
        public string FileType { get; set; }
        public CustomerFlow Customer { get; set; }
    }
}
