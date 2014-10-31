﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexyBox.ViewModel
{
    public class StepGroupViewModel
    {
        public int Id { get; set; }
        public string Header {get; set;}
        public BindingList<StepQuestionViewModel> Questions { get; set; }

    }
}