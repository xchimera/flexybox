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
        public MainWindow()
        {
            InitializeComponent();
            Model = new MainWindowViewModel();
            //Loaded += MainWindow_Loaded;
            Reload();
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            using (var ctx = new FlexyboxContext())
            {
                StepAnswer answer = new StepAnswer() { QuestionId = 4, TimeChanged = DateTime.Now, EmployeeId = 1 };
                ctx.SaveEntity<StepAnswer>(answer);
                ctx.SaveEntity<StepQuestion>(new StepQuestion() { GroupId = 1, DateCreated = DateTime.Now, Description = "Test", Header = "TestTest", Order = 0, AnswerId = answer.Id });
            }
        }

        private void Reload()
        {
            Model.Groups.Clear();
            List<StepGroupViewModel> result;
            using (var ctx = new FlexyboxContext())
            {
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
                        Description = c.Description,
                        Header = c.Header,
                        Id = c.Id,
                        Order = c.Order,
                        
                    }).ToList()));


                foreach(var item in result)
                {
                    foreach(var question in item.Questions)
                    {
                        question.Answer = ctx.Query<StepAnswer>().Where(x => x.QuestionId == question.Id && !x.IsLog)
                            .Select(c => new StepAnswerViewModel()
                            {
                                Id = c.Id,
                                Answer = c.QuestionAnswer,
                                Comment = c.Comment,
                                EmployeeId = c.EmployeeId,
                                TimeChanged = c.TimeChanged
                            }).SingleOrDefault();
                        question.Answer.Question = question;
                    }
                }

            }
            Model.Groups.AddRange(result);
        }

        private void Completed_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var answer = ((sender as Image).DataContext as StepQuestionViewModel).Answer;  
        }

        private void NotCompleted_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void CheckBox_MouseUp(object sender, MouseButtonEventArgs e)
        {


        }
    }

    public class MainWindowViewModel
    {
        public ObservableCollection<StepGroupViewModel> Groups { get; set; }

        public MainWindowViewModel()
        {
            Groups = new ObservableCollection<StepGroupViewModel>();
        }

    }
}
