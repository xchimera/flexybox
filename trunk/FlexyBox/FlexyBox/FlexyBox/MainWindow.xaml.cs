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
                ctx.SaveEntity<StepQuestion>(new StepQuestion() { GroupId = 1, DateCreated = DateTime.Now, Description = "Test", Header = "TestTest", Order = 0, AnswerId = answer.Id, Question = new StepQuestion() { Id = 1 } });
            }
        }

        private void Reload(int customerId)
        {
            Model.Groups.Clear();
            List<StepGroupViewModel> result;
            using (var ctx = new FlexyboxContext())
            {
                Model.CustomerViewModel = ctx.QueryFromID<CustomerFlow>(customerId).Select(x => new CustomerFlowViewModel()
                    {
                        Id = x.Id,
                        CustomerId = x.CustomerId,
                        Name = x.Name
                    }).SingleOrDefault();

                result = (ctx.Query<StepGroup>()
                    .Select(x => new StepGroupViewModel()
                {
                    Header = x.Header,
                    Id = x.Id,
                })).ToList();

                result.ForEach(x => x.Questions = new BindingList<StepQuestionViewModel>(ctx.Query<StepQuestion>()
                    .Where(y => y.GroupId == x.Id)
                    .Select(c => new StepQuestionViewModel()
                    {
                        Entity = c,
                        //Child = c.Question.Id
                        
                    }).ToList()));
                

                foreach(var item in result)
                {
                    
                    foreach(var question in item.Questions)
                    {
                        
                            //if(question.Entity.Question != null)
                            //    question.Entity.Question.
                        question.Answer = ctx.Query<StepAnswer>().Where(x => x.QuestionId == question.Id && !x.IsLog && x.CustomerFlowId == Model.CustomerViewModel.Id)
                            .Select(c => new StepAnswerViewModel()
                            {
                                Entity = c,
                            }).SingleOrDefault();
                        question.Answer.Question = question;
                        question.Answer.Group = item;
                    }
                }
            }
            Model.Groups.AddRange(result);
            
        }



        private void CheckBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var answer = ((sender as Image).DataContext as StepQuestionViewModel).Answer;
            if (e.LeftButton == MouseButtonState.Released || e.RightButton == MouseButtonState.Released)
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
                Console.WriteLine((after-before).TotalMilliseconds);

                StepAnswer newAnswer = new StepAnswer()
                {
                    Comment = answer.Comment,
                    CustomerFlowId = Model.CustomerViewModel.Id,
                    EmployeeId = Model.EmployeeId,
                    QuestionAnswer = answer.Answer,
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


                if (e.LeftButton == MouseButtonState.Released)
                {
                    if (answer.Answer == EnumAnswer.Done)
                        answer.Answer = EnumAnswer.NotDone;
                    else if (answer.Answer == EnumAnswer.NotDone)
                        answer.Answer = EnumAnswer.NotApplicable;
                    else if (answer.Answer == EnumAnswer.NotApplicable)
                        answer.Answer = EnumAnswer.Done;
                    else if (answer.Answer == EnumAnswer.NotAnswered)
                        answer.Answer = EnumAnswer.Done;
                }
                else
                {
                    if (answer.Answer == EnumAnswer.Done)
                        answer.Answer = EnumAnswer.NotApplicable;
                    else if (answer.Answer == EnumAnswer.NotApplicable)
                        answer.Answer = EnumAnswer.NotDone;
                    else if (answer.Answer == EnumAnswer.NotDone)
                        answer.Answer = EnumAnswer.Done;
                    else if (answer.Answer == EnumAnswer.NotAnswered)
                        answer.Answer = EnumAnswer.Done;
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
            LogWindow popup = new LogWindow();       
            popup.ShowDialog();
        }

    }

    public class MainWindowViewModel
    {
        public int EmployeeId { get; set; }
        public ObservableCollection<StepGroupViewModel> Groups { get; set; }
        public CustomerFlowViewModel CustomerViewModel { get; set; }
        public MainWindowViewModel(int employeeId)
        {
            Groups = new ObservableCollection<StepGroupViewModel>();
            EmployeeId = employeeId;
        }

    }
}
