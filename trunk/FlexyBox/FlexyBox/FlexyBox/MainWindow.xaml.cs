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
            Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var group = new StepGroup()
            {
                Header = "Pre-Installation"
            };

            using (var ctx = new FlexyboxContext())
            {
                ctx.SaveEntity<StepGroup>(group);
            }


        }

        private void Completed_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void NotCompleted_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void NotConsidered_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }
    }

    public class MainWindowViewModel
    {

    }
}
