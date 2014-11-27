using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexyDomain.Models
{
    public class Product : EntityPersist
    {
        public string Name { get; set; }
        public IList<CustomerFlow> Customers { get; set; }

    }
}
