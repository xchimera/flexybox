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

        public int Id
        {
            get
            {
                return Entity.Id;
            }
        }
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
        public FlexyDomain.Models.EnumAnswer Answer
        {
            get
            {
                return Entity.QuestionAnswer;
            }
            set
            {
                Entity.QuestionAnswer = value;
                OnPropertyChanged("Answer");
            }
        }
        public int EmployeeId { get; set; }
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
        public StepQuestionViewModel Question { get; set; }
        public StepGroupViewModel Group { get; set; }
        public bool IsChild
        {
            get
            {
                if (Question.Parent != null)
                    return true;
                return false;
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
