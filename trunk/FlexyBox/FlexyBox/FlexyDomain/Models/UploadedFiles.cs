using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexyDomain.Models
{
    public class UploadedFiles : EntityPersist
    {
        public string Name { get; set; }
        public virtual byte[] File { get; set; }

    }
}
