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
        {
            get
            {
                return Entity.Id;
            }
        }

        public string Header
        {
            get
            {
                return Entity.Header;
            }
            set
            {
                Entity.Header = value;
                OnPropertyChanged("Header");
            }
        }
        public string Description
        {
            get
            {
                return Entity.Description;
            }
            set
            {
                Entity.Description = value;
                OnPropertyChanged("Description");
            }
        }

        public int Order
        {
            get
            {
                return Entity.Order;
            }
            set
            {
                Entity.Order = value;
                OnPropertyChanged("Order");
            }
        }

        public BindingList<StepQuestionViewModel> Children { get; set; }

        public int Child { get; set; }

        public bool IsChild
        {
            get
            {
                if (Parent != 0)
                    return true;
                return false;
            }
        }

        public int Parent
        {
            get
            {
                if (Entity.Parent != null)
                    return Entity.Parent.Id;
                return 0;
            }

        }
        private StepAnswerViewModel _answer;
        public StepAnswerViewModel Answer
        {
            get
            {
                return _answer;
            }
            set
            {
                if (_answer != null)
                    _answer.PropertyChanged -= _answer_PropertyChanged;

                _answer = value;
                _answer.PropertyChanged += _answer_PropertyChanged;
            }
        }

        void _answer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("Answer");
        }
        public StepGroupViewModel Group { get; set; }


        public StepQuestionViewModel()
        {
            Children = new BindingList<StepQuestionViewModel>();
        }
        public void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }

        }
    }
}
