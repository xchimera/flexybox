using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexyBox.ViewModel
{
    public class UploadedFilesViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] File { get; set; }
    }
}
