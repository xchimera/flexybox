using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace FlexyBox.ViewModel
{
    //Lavet af Vijeeth
    public class QuestionVisibilityViewModel
    {
        public int Id { get; set; }
        public StepQuestionViewModel Question { get; set; }
        public BindingList<StepQuestionViewModel> Questions { get; set; }

        public QuestionVisibilityViewModel()
        {
            Questions = new BindingList<StepQuestionViewModel>();
        }
    }
}
