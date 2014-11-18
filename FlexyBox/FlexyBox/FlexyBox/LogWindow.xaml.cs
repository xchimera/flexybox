using FlexyBox.ViewModel;
using FlexyDomain;
using FlexyDomain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FlexyDomain.Extensions;

namespace FlexyBox
{
    /// <summary>
    /// Interaction logic for LogWindow.xaml
    /// </summary>
    /// 
    public partial class LogWindow : Window
    {
        public LogWindowViewModel Model
        {
            get
            {
                return DataContext as LogWindowViewModel;
            }
            set
            {
                DataContext = value;
            }
        }
        public LogWindow(int questionId)
        {
            InitializeComponent();
            Model = new LogWindowViewModel();
            Model.QuestionId = questionId; 
            MouseLeave += LogWindow_MouseLeave;
        }

        private void LogLoad()
        {
            List<StepAnswerViewModel> result = new List<StepAnswerViewModel>();
            using(FlexyboxContext ctx = new FlexyboxContext())
            {
                result = ctx.Query<StepAnswer>().Where(x => x.IsLog == true && x.QuestionId==Model.QuestionId).Select(x => new StepAnswerViewModel()
                    {
                        Entity = x
                    }).ToList();
            }

            Model.LogGroups = result.OrderByDescending(x=> x.TimeChanged).ToBindingList();
        }

        void LogWindow_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Close();
        }
    }

    public class LogWindowViewModel
    {
        public BindingList<StepAnswerViewModel> LogGroups { get; set; }
        public int QuestionId { get; set; }

        public LogWindowViewModel()
        {
            LogGroups = new BindingList<StepAnswerViewModel>();
        }
    }
}
