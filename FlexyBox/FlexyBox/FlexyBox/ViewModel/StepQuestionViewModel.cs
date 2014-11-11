using FlexyDomain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexyBox.ViewModel
{
    public class StepQuestionViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public StepQuestion Entity { get; set; }


        public int Id 
        public string Header{ get; set; }
        public string Description{ get; set; }
        public int Order { get; set; }
        public int Child { get; set; }
        public int Parent { get; set; }
        public StepAnswerViewModel Answer { get; set; }
        public StepGroupViewModel Group { get; set; }


    }
}
