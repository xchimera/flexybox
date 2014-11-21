using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexyBox.ViewModel
{
    public class CustomerFlowViewModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public List<ProductViewModel> Products { get; set; }

        public CustomerFlowViewModel()
        {
            Products = new List<ProductViewModel>();
        }
    }
}
