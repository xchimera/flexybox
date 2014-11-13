using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FlexyDomain;
using FlexyDomain.Models;
using System.ComponentModel;
using FlexyBox.ViewModel;
using System.Collections.ObjectModel;
using FlexyDomain.Extensions;
using System.Diagnostics;
using System.Data.Entity;

namespace FlexyBox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindowViewModel Model
        {
            get { return DataContext as MainWindowViewModel; }
            set { DataContext = value; }
        }
        public MainWindow(int employeeId, int customerId)
        {
            InitializeComponent();
            Model = new MainWindowViewModel(employeeId);
            //Loaded += MainWindow_Loaded;
            Reload(customerId);
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            using (var ctx = new FlexyboxContext())
            {
                StepAnswer answer = new StepAnswer() { QuestionId = 6, TimeChanged = DateTime.Now, EmployeeId = 1 };
                ctx.SaveEntity<StepAnswer>(answer);
                ctx.SaveEntity<StepQuestion>(new StepQuestion()
                {
                    GroupId = 1,
                    DateCreated = DateTime.Now,
                    Description = "Test",
                    Header = "TestTest",
                    Order = 0,
                });
            }
        }

        private List<StepAnswerViewModel> GetAnswers(FlexyboxContext ctx, IList<StepAnswer> answers)
        {
            var result = new List<StepAnswerViewModel>();
            foreach (var answer in answers)
            {
                var answerVm = new StepAnswerViewModel()
                {
                    Entity = answer,
                };
                result.Add(answerVm);
            }


            return result;
        }

        private List<StepGroupViewModel> GetGroups(FlexyboxContext ctx)
        {
            var result = new List<StepGroupViewModel>();

            var groups = ctx.Query<StepGroup>()
                .Include(x => x.Questions)
                .ToList();

            foreach (var group in groups)
            {

                var groupVm = new StepGroupViewModel()
                {
                    Id = group.Id,
                    Header = group.Header,

                    Questions = group.Questions.Select(x => new StepQuestionViewModel()
                    {
                        Entity = x,
                    }).ToBindingList(),


                };
                groupVm.Questions.ForEach(x => x.Group = groupVm);
                result.Add(groupVm);
            }

            var toDelete = new List<StepQuestionViewModel>();
            
            
            foreach(var group in result)
            {
                foreach(var question in group.Questions)
                {
                    if(question.Parent != 0)
                    {
                        
                        group.Questions
                            .SingleOrDefault(x => x.Id == question.Parent)
                            .Children.Add(question);
                        toDelete.Add(question);
                    }
                }
            }

            foreach(var question in toDelete)
            {
                result.ForEach(x => x.Questions.Remove(question));
            }

            return result;
        }

        private void Reload(int customerId)
        {
            Model.Groups.Clear();

            List<StepAnswerViewModel> answers;
            CustomerFlowViewModel customer;
            List<StepGroupViewModel> groups;
            using (var ctx = new FlexyboxContext())
            {
                customer = ctx.QueryFromID<CustomerFlow>(customerId).Select(x => new CustomerFlowViewModel()
                {
                    Id = x.Id,
                    CustomerId = x.CustomerId,
                    Name = x.Name
                }).Single();

                var entityAnswers = ctx.Query<StepAnswer>().Where(x => x.CustomerFlowId == customer.Id && !x.IsLog).ToList();

                groups = GetGroups(ctx);
                answers = GetAnswers(ctx, entityAnswers);

            }
            foreach (var group in groups)
            {
                foreach (var question in group.Questions)
                {
                    foreach (var answer in answers)
                    {
                        if (answer.QuestionId == question.Id)
                        {
                            question.Answer = answer;
                            answer.Question = question;
                            break;
                        }
                    }
                }
            }
            Model.CustomerViewModel = customer;
            Model.Groups = groups.ToBindingList();

        }


        
        private void CheckBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var answer = ((sender as Image).DataContext as StepQuestionViewModel).Answer;
            if (e.LeftButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed)
            {
                answer.TimeChanged = DateTime.Now;
                answer.EmployeeId = Model.EmployeeId;
                answer.IsLog = true;
                var before = DateTime.Now;
                using (var ctx = new FlexyboxContext())
                {
                    if (!ctx.SaveEntity<StepAnswer>(answer.Entity))
                        MessageBox.Show("Fejl i at gemme loggen");
                }
                var after = DateTime.Now;
                Console.WriteLine((after - before).TotalMilliseconds);

                StepAnswer newAnswer = new StepAnswer()
                {
                    Comment = answer.Comment,
                    CustomerFlowId = Model.CustomerViewModel.Id,
                    EmployeeId = Model.EmployeeId,
                    State = answer.State,
                    QuestionId = answer.Question.Id,
                    TimeChanged = DateTime.Now

                };

                answer = new StepAnswerViewModel() { Entity = newAnswer, Question = answer.Question };
                StepQuestionViewModel usedQuestion = null;

                foreach (var group in Model.Groups)
                {
                    var shouldBreak = false;
                    foreach (var question in group.Questions)
                    {
                        if (question.Id == answer.Question.Id)
                        {
                            question.Answer = answer;
                            shouldBreak = true;
                            break;
                        }
                    }
                    if (shouldBreak)
                        break;
                }


                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    if (answer.State == AnswerState.Done)
                        answer.State = AnswerState.NotDone;
                    else if (answer.State == AnswerState.NotDone)
                        answer.State = AnswerState.NotApplicable;
                    else if (answer.State == AnswerState.NotApplicable)
                        answer.State = AnswerState.Done;
                    else if (answer.State == AnswerState.NotAnswered)
                        answer.State = AnswerState.Done;
                }
                else
                {
                    if (answer.State == AnswerState.Done)
                        answer.State = AnswerState.NotApplicable;
                    else if (answer.State == AnswerState.NotApplicable)
                        answer.State = AnswerState.NotDone;
                    else if (answer.State == AnswerState.NotDone)
                        answer.State = AnswerState.Done;
                    else if (answer.State == AnswerState.NotAnswered)
                        answer.State = AnswerState.Done;
                }


                using (var ctx = new FlexyboxContext())
                {
                    if (!ctx.SaveEntity<StepAnswer>(newAnswer))
                        MessageBox.Show("Fejl i at gemme ny svar entitet");
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LogWindow logWindow = new LogWindow();
            var loc = (sender as Button).PointToScreen(new Point(0, 0));
            logWindow.Left = loc.X - logWindow.Width;
            logWindow.Top = loc.Y;
            logWindow.Show();
           
        }

        public class MainWindowViewModel
        {
            public int EmployeeId { get; set; }
            public BindingList<StepGroupViewModel> Groups { get; set; }
            public CustomerFlowViewModel CustomerViewModel { get; set; }
            public MainWindowViewModel(int employeeId)
            {
                Groups = new BindingList<StepGroupViewModel>();
                EmployeeId = employeeId;
            }

        }
    }
}
