using FlexyDomain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexyBox.ViewModel
{
    public class StepAnswerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public StepAnswer Entity { get; set; }

        public string Comment
        {
            get
            {
                return Entity.Comment;
            }
            set
            {
                Entity.Comment = value;
                OnPropertyChanged("Comment");
            }
        }
        public FlexyDomain.Models.AnswerState State
        {
            get
            {
                return Entity.State;
            }
            set
            {
                Entity.State = value;
                OnPropertyChanged("State");
            }
        }
        public int EmployeeId { get; set; } //hent fra entity
        public DateTime TimeChanged
        {
            get
            {
                return Entity.TimeChanged;
            }
            set
            {
                Entity.TimeChanged = value;
                OnPropertyChanged("TimeChanged");
            }
        }
        public bool IsLog
        {
            get
            {
                return Entity.IsLog;
            }
            set
            {
                Entity.IsLog = value;
                OnPropertyChanged("IsLog");
            }
        }



        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }

        }

    }
}
