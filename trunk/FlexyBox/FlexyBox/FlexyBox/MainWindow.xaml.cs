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
namespace FlexyBox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //StepQuestion question = new StepQuestion()
            //{
            //    DateCreated = DateTime.Now,
            //    Description = "tralalal",
            //    Header = "endnu mere tralalal",
            //    Order = 0
            //};
            //StepAnswer answer = new StepAnswer()
            //{
            //    Comment = "ad",
            //    EmployeeId = 1,
            //    IsLog = false,
            //    Question = question,
            //    QuestionAnswer = Answer.Done,
            //    TimeChanged = DateTime.Now
            //};
            using (var ctx = new FlexyboxContext())
            {

                var asd = ctx.Query<StepAnswer>(false);//.ToList();//.Where(x => x.IsDeleted != true).ToList();
                var aasd = asd.ToList();
                loader.Text = aasd.Count + " hentet";
            }
            


        }
    }
}
