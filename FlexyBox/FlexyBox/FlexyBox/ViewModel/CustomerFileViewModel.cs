using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexyBox.ViewModel
{
    public class CustomerFileViewModel
    {
        public int Id { get; set; }
        public string FileType { get; set; }
        public byte[] File { get; set; }
    }
}
