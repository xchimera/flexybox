using FlexyDomain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexyBox.ViewModel
{
    public class StepLogViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public StepAnswer Entity { get; set; }

        public int Id
        {
            get
            {
                return Entity.Id;
            }
        }
    }
}
