﻿using FlexyDomain.Models;
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

        public int Child { get; set; }
        public int Parent { get; set; }
        public StepAnswerViewModel Answer { get; set; }
        public StepGroupViewModel Group { get; set; }

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
