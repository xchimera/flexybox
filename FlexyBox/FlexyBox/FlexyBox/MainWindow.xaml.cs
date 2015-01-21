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
using System.Windows.Controls.Ribbon;
using System.IO;

namespace FlexyBox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    //Lavet af Søren
    public partial class MainWindow : RibbonWindow
    {
        //Lav en reference til View modellen og sæt den som vinduets datacontext
        public MainWindowViewModel Model
        {
            get { return DataContext as MainWindowViewModel; }
            set { DataContext = value; }
        }
        public MainWindow(int employeeId, int customerId)
        {
            InitializeComponent();
            Model = new MainWindowViewModel(employeeId);
            Reload(customerId);
        }

        private List<StepAnswerViewModel> GetAnswers(IList<StepAnswer> answers)
        {
            var result = new List<StepAnswerViewModel>();
            //Kør alle svarene igennem og lav en ViewModel for hver
            foreach (var answer in answers)
            {
                var answerVm = new StepAnswerViewModel()
                {
                    Entity = answer,
                    Comment = answer.Comment,

                };
                result.Add(answerVm);
            }


            return result;
        }

        private List<StepGroupViewModel> GetGroupsWithQuestions(FlexyboxContext ctx, CustomerFlow customer)
        {
            //find produkternes Id
            var productIds = customer.Products.Select(x => x.Id).ToList();

            //hent Visibility entiteten ud og inkluder de Questions der passer sammen med den og skab view modeller
            //disse bliver ikke brugt endnu da funktionen desværre ikke er færdig og skal optimeres
            var visibilities = ctx.Query<QuestionVisibility>()
                .Include(x => x.Question)
                .Include(x => x.Questions)
                .Where(x => productIds.Contains(x.Question.Product.Id))
                .ToList()
                .Select(x => new QuestionVisibilityViewModel()
                {
                    Id = x.Id,
                    Question = new StepQuestionViewModel() { Entity = x.Question},
                    Questions = x.Questions.Select(c => new StepQuestionViewModel()
                    {
                        Entity = c

                    })
                    .ToBindingList(),
                })
                .ToList();
            //hent alle spørgsmål ud og inkluder de grupper de skal være i, 
            //grupper derefter spørgsmålene efter grupperne og lav view modeller
            var result = ctx.Query<StepQuestion>()
                .Include(x => x.Group)
                .Where(x => productIds.Contains(x.Product.Id))
                .GroupBy(x => x.Group)
                .ToList()
                .Select(x => new StepGroupViewModel()
                {
                    Id = x.Key.Id,
                    Header = x.Key.Header,
                    Questions = x.Select(c => new StepQuestionViewModel()
                    {
                        Entity = c,
                    })
                    .ToBindingList(),
                })
                .ToList();

            var questionToDelete = new List<StepQuestionViewModel>();
            var visibilityToDelete = new List<QuestionVisibilityViewModel>();
            //kør alle spørgmsål igennem og tilføj de spørgsmål der er underspørgsmål til deres "parent
            //tilføj dem derefter til en liste for senere at slet dem da dette ikke kan gøres under itereringen
            foreach (StepGroupViewModel group in result)
            {
                foreach (var question in group.Questions)
                {

                    if (question.Parent != 0)
                    {
                        group.Questions
                            .SingleOrDefault(x => x.Id == question.Parent)
                            .Children.Add(question);
                        questionToDelete.Add(question);
                    }
                    
                    //foreach(var visibility in visibilities)
                    //{
                    //    if (visibility.Question.Id == question.Id)
                    //        visibility.Question = question;
                    //    var questionLoc = visibility.Questions.IndexOf(question);
                    //    if(questionLoc >= 0)
                    //        visibility.Questions[questionLoc] = question;

                    //}
                }
                group.Questions = group.Questions.OrderByDescending(x => x.Order).ToBindingList();
            }
            //slet under spørgsmålene så de ikke bliver vist to gange
            foreach (var question in questionToDelete)
            {
                result.ForEach(x => x.Questions.Remove(question));
            }

            return result;
        }

        private CustomerFlowViewModel GetCustomer(CustomerFlow customer)
        {
            //Skab en ViewModel til CustomerFlowet
            var customerVM = new CustomerFlowViewModel()
            {
                Id = customer.Id,
                Name = customer.Name,
                CustomerId = customer.CustomerId,
                Entity = customer,
            };
            //Skab og tilføj produkt ViewModels 
            foreach (var product in customer.Products)
            {
                customerVM.Products.Add(new ProductViewModel()
                    {
                        Id = product.Id,
                        Name = product.Name
                    });
            }

            return customerVM;

        }


        private void Reload(int customerId)
        {
            Model.Groups.Clear();

            List<StepAnswerViewModel> answers;
            CustomerFlowViewModel customer;
            List<StepGroupViewModel> groups;
            using (var ctx = new FlexyboxContext())
            {
                //hent kunden ud fra dens Id, og inkluder dennes produkter
                var entCustomer = ctx.QueryFromID<CustomerFlow>(customerId)
                    .Include(x => x.Products).Single();

                customer = GetCustomer(entCustomer);
                //hent svarene der passer til kunden
                var entityAnswers = ctx.Query<StepAnswer>().Where(x => x.CustomerFlow.Id == customer.Id && !x.IsLog).ToList();

                answers = GetAnswers(entityAnswers);
                groups = GetGroupsWithQuestions(ctx, entCustomer);
            }

            foreach (var group in groups)
            {
                //kør alle grupper igennem og tilføj svarene, samt kør alle underspørgsmål igennem og tilføj svarene
                foreach (var question in group.Questions)
                {
                    foreach (var child in question.Children)
                        child.Answer = answers.Single(x => x.Entity.QuestionId == child.Id);
                    //question.Answer = InsertAnswers(question, answers);


                    var answer = answers.Single(x => x.Entity.QuestionId == question.Id);
                    question.Answer = answer;
                }
            }

            Model.CustomerViewModel = customer;
            Model.Groups = groups.ToBindingList();

        }

        private void LoadFiles()
        {
            List<MemoryStream> files = new List<MemoryStream>();
            //denne funktion er desværre ikke færdig
            using (var ctx = new FlexyboxContext())
            {
                //hent de filer der er tilknyttet kunden ud og lav en view model
                var result = ctx.Query<CustomerFile>().Where(x => x.Customer.Id == Model.CustomerViewModel.CustomerId)
                    .Select(x => new CustomerFileViewModel()
                    {
                        File = x.File,
                        Id = x.Id,
                        FileType = x.FileType,
                    }).ToList();
                foreach(var file in result)
                {
                    files.Add(new MemoryStream(file.File));
                }
            }
            
        }

        private void CheckBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //find det tilknyttede spørgsmål ud fra billedets datacontext
            var questionVm = (sender as Image).DataContext as StepQuestionViewModel;
            //hiv svar objektet ud
            var answer = questionVm.Answer;
            //tjek at det ikke er andet en venstre eller højre mussetast der er klikket
            if (e.LeftButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed)
            {
                //sæt tiden på loggen
                answer.TimeChanged = DateTime.Now;
                //sæt hvem der har ændret svaret
                answer.EmployeeId = Model.EmployeeId;
                //sæt svaret til at være en log
                answer.IsLog = true;
                //debug kode for at se hvor lang tid det tager at gemme
                var before = DateTime.Now;
                using (var ctx = new FlexyboxContext())
                {
                    //gem entiteten med et tjek om det gik godt
                    if (!ctx.SaveEntity<StepAnswer>(answer.Entity))
                        MessageBox.Show("Fejl i at gemme loggen");
                }
                //debug kode for at se hvor lang tid det tager at gemme
                var after = DateTime.Now;
                //regn ud hvor længe det tog at gemme loggen
                Console.WriteLine((after - before).TotalMilliseconds);
                //skab et nyt svar objekt der vil være den aktive entitet for spørgsmålet
                StepAnswer newAnswer = new StepAnswer()
                {
                    Comment = answer.Comment,
                    CustomerFlow = Model.CustomerViewModel.Entity,
                    EmployeeId = Model.EmployeeId,
                    State = answer.State,
                    QuestionId = questionVm.Id,
                    TimeChanged = DateTime.Now

                };
                //lav en view model til det nye aktive svar
                answer = new StepAnswerViewModel() { Entity = newAnswer, Comment = answer.Comment };
                //sæt det nye svars view model til spørgsmålets svar property
                questionVm.Answer = answer;

                //tjek hvilken vej stadierne skal køre
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    //tjek det nuværende stadie og skift stadiet derefter
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
                    //gem den nye svar entitet og tjek for svar.
                    ctx.Entry(newAnswer.CustomerFlow).State = EntityState.Unchanged;
                    if (!ctx.SaveEntity<StepAnswer>(newAnswer))
                        MessageBox.Show("Fejl i at gemme ny svar entitet");
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var questionId = ((sender as Button).DataContext as StepQuestionViewModel).Id;
            LogWindow logWindow = new LogWindow(questionId);
            var loc = (sender as Button).PointToScreen(new Point(0, 0));
            logWindow.Left = loc.X - logWindow.Width;
            logWindow.Top = loc.Y;
            logWindow.Show();

        }



        private void ManageFiles_Click(object sender, RoutedEventArgs e)
        {
            //vis den nye fil manager
            FileManager fileManager = new FileManager(Model.CustomerViewModel);
            fileManager.ShowDialog();
        }

        private void EditCustomer_Click(object sender, RoutedEventArgs e)
        {
            //vis rediger kunde dialogen og vis der bliver trykket ok på dialogen, opdater alle spørgsmål og grupper
            if (new NewCustomer(Model.CustomerViewModel.Entity, Model.EmployeeId).ShowDialog() == true)
                Reload(Model.CustomerViewModel.Id);
        }



        private void Comment_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //lås kommentar textboxen op
            ((sender as TextBox).DataContext as StepQuestionViewModel).Answer.CanEdit = true;

        }

        private void SaveComment_Click(object sender, RoutedEventArgs e)
        {
            var answer = ((sender as Button).DataContext as StepQuestionViewModel).Answer;
            answer.Entity.Comment = answer.Comment;
            //gem kommentaren på det nuværende svar
            //dette skal laves om til at logge det gamle svar og lave en ny svar entitet som kommentaren skal gemmes på
            using (var ctx = new FlexyboxContext())
            {
                if (!ctx.SaveEntity(answer.Entity))
                    MessageBox.Show("Kunne ikke gemme svaret!");
            }
            answer.CanEdit = false;
        }

        private void UndoComment_Click(object sender, RoutedEventArgs e)
        {
            var answer = ((sender as Button).DataContext as StepQuestionViewModel).Answer;
            //hvis sæt kommentaren til hvad den var tidligere
            answer.Comment = answer.Entity.Comment;
            answer.CanEdit = false;
        }



        private void RibbonWindow_DragEnter(object sender, DragEventArgs e)
        {
            //denne metode er ikke færdig
            Model.TabState = TabState.FileState;
            //skift tab og hent informationer om filerne som bliver trukket ind
            string[] files = (string[])(e.Data.GetData(DataFormats.FileDrop, false));

            foreach (var file in files)
            {
                if (System.IO.Path.GetExtension(file) != ".docx")
                {

                }
            }

                
        }

        private void RemoveFile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Ribbon_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //skift tab alt efter hvad den nuværende tab er
            var ribbon = (sender as RibbonTab);
            if (Model.TabIndex == 0)
                Model.TabState = TabState.QuestionState;
            else
            {
                Model.TabState = TabState.FileState;
                LoadFiles();
            }
        }


        private void fileGrid_Drop(object sender, DragEventArgs e)
        {

        }

        

    }
    public class MainWindowViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public BindingList<StepGroupViewModel> Groups { get; set; }
        public CustomerFlowViewModel CustomerViewModel { get; set; }
        public BindingList<MemoryStream> Files { get; set; }
        public int EmployeeId { get; set; }

        private TabState _TabState;
        public TabState TabState
        {
            get
            {
                return _TabState;
            }
            set
            {
                _TabState = value;
                //giv besked om at tab stadiet har ændret sig
                OnPropertyChanged("TabState");
            }
        }

        public int TabIndex { get; set; }

        public MainWindowViewModel(int employeeId)
        {
            Groups = new BindingList<StepGroupViewModel>();
            EmployeeId = employeeId;
            TabState = TabState.QuestionState;

        }

        public void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                //giv besked til vinduet om at noget har ændret sig
                handler(this, new PropertyChangedEventArgs(name));
            }

        }

    }

    public enum TabState
    {
        QuestionState, FileState
    }
}
