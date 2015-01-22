using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using FlexyDomain.Extensions;
using System.Threading.Tasks;

namespace FlexyBox.ViewModel
{
    //Lavet af Vijeeth
    public class StepGroupViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get; set; }
        public string Header {get; set;}
        public BindingList<StepQuestionViewModel> Questions { get; set; }

        private int _NumberOfQuestions;
        public int NumberOfQuestions
        {
            get { return _NumberOfQuestions; }
            set
            {
                _NumberOfQuestions = value;
                OnPropertyChanged("NumberOfQuestions");
            }
        }

        private int _QuestionsAnswered;
        public int QuestionsAnswered
        { 
            get
            {
                return _QuestionsAnswered;
            }
            set
            {
                _QuestionsAnswered = value;
                OnPropertyChanged("QuestionsAnswered");
            }
        }

        int _CalculatedPercentage;
        public int CalculatedPercentage
        {
            get
            {
                return _CalculatedPercentage;
            }
            set
            {
                _CalculatedPercentage = value;
                OnPropertyChanged("CalculatedPercentage");
            }
        }

        public StepGroupViewModel()
        {
            Initialize();
        }

        private void Initialize()
        {
            this.PropertyChanged += StepGroupViewModel_PropertyChanged;
        }

        void Questions_ListChanged(object sender, ListChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void StepGroupViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case "State":
                    this.Update();
                    break;
            }
        }

        public void Update()
        {
            var result = 0;

            foreach (var question in Questions)
            {
                result += question.Children.Count;
                result++;
            }
            NumberOfQuestions = result;

            
            result = 0;

            foreach (var question in Questions)
            {
                if (question.Answer.State != FlexyDomain.Models.AnswerState.NotAnswered)
                    result++;
                result += question.Children.Count(x => x.Answer.State != FlexyDomain.Models.AnswerState.NotAnswered);
            }
            QuestionsAnswered = result;

            result = 0;

            result = (int)Math.Round((double)(100 * QuestionsAnswered) / NumberOfQuestions);

            CalculatedPercentage = result;

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
