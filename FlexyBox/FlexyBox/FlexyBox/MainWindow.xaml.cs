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
          //  Loaded += MainWindow_Loaded;
            Reload();
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var group = new StepGroup()
            {
                Header = "Oplysning og Hardware"
            };
            var group1 = new StepGroup()
            {
                Header = "Server installation"
            };
            var group2 = new StepGroup()
            {
                Header = "Vigtig opsætning"
            };
            var group3 = new StepGroup()
            {
                Header = "Øvrig opsætning"
            };
            var group4 = new StepGroup()
            {
                Header = "3 dage før installationen"
            };
            var group5 = new StepGroup()
            {
                Header = "1 dag før installationen"
            };
            var group6 = new StepGroup()
            {
                Header = "På installationsdage"
            };
            var group7 = new StepGroup()
            {
                Header = "Hjemme, umidelbart installationen"
            };
            var group8 = new StepGroup()
            {
                Header = "2 uger efter installationen"
            };
            var group9 = new StepGroup()
            {
                Header = "3 måneder efter installationen"
            };

            using (var ctx = new FlexyboxContext())
            {
                ctx.SaveEntity<StepGroup>(group);
                ctx.SaveEntity<StepGroup>(group1);
                ctx.SaveEntity<StepGroup>(group2);
                ctx.SaveEntity<StepGroup>(group3);
                ctx.SaveEntity<StepGroup>(group4);
                ctx.SaveEntity<StepGroup>(group5);
                ctx.SaveEntity<StepGroup>(group6);
                ctx.SaveEntity<StepGroup>(group7);
                ctx.SaveEntity<StepGroup>(group8);
                ctx.SaveEntity<StepGroup>(group9);
            }


        }

        private void Reload()
        {
            using (var ctx = new FlexyboxContext())
            {
                Model.Groups.AddRange(ctx.Query<StepGroup>()
                    .Select(x => new StepGroupViewModel()
                {
                    Header = x.Header,
                    Id = x.Id
                }));
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
        public ObservableCollection<StepGroupViewModel> Groups { get; set; }

        public MainWindowViewModel()
        {
            Groups = new ObservableCollection<StepGroupViewModel>();
        }

    }
}
