using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexyBox.ViewModel
{
    //Lavet af Vijeeth
    public class CustomerFileViewModel
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public byte[] File { get; set; }
    }
}
