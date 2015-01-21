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

        public int NumberOfQuestions
        {
            get
            {
                var result = 0;
                
                foreach (var question in Questions)
                {
                    result += question.Children.Count;
                    result++; 
                }

                return result;  
            }
        }

        public int QuestionsAnswered
        { 
            get
            {
                var result = 0;

                foreach( var question in Questions)
                {
                    if (question.Answer.State != FlexyDomain.Models.AnswerState.NotAnswered)
                        result++;
                    result += question.Children.Count(x => x.Answer.State != FlexyDomain.Models.AnswerState.NotAnswered);  
                }
                return result;
            }
        }
        public int CalculatedPercentage
        {
            get
            {
                var numberOfQuestions = Questions.Count;
                var questionsAnswered = Questions.Count(x => x.Answer.State != FlexyDomain.Models.AnswerState.NotAnswered);
                var toreturn = (int)Math.Round((double)(100 * questionsAnswered) / numberOfQuestions);          

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
