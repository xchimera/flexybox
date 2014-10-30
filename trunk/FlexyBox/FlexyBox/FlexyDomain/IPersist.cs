using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexyDomain
{
    public interface IPersist
    {
        bool IsDeleted { get; set; }
    }
}
