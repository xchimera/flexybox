using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using FlexyDomain.Extensions;
using System.Threading.Tasks;

namespace FlexyBox.ViewModel
{
    public class StepGroupViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get; set; }
        public string Header {get; set;}
        public BindingList<StepQuestionViewModel> Questions { get; set; }

        public int CalculatedPercentage
        {
            get
            {
                var numberOfQuestions = Questions.Count;
                var questionsAnswered = Questions.Count(x => x.Answer.State != FlexyDomain.Models.AnswerState.NotAnswered);
                var toreturn = ((questionsAnswered / numberOfQuestions) * 100);

                return toreturn;
                }
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
