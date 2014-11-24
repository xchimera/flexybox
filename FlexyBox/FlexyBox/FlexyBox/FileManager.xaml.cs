using FlexyBox.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using FlexyDomain;
using FlexyDomain.Models;
namespace FlexyBox
{
    /// <summary>
    /// Interaction logic for FileManager.xaml
    /// </summary>
    public partial class FileManager : Window
    {
        public FileManagerViewModel Model
        {
            get { return DataContext as FileManagerViewModel; }
            set { DataContext = value; }
        }
        public FileManager(int customerId)
        {
            InitializeComponent();
            Model = new FileManagerViewModel();
            Model.CustomerId = customerId;
        }

        private void Reload()
        {
            var result = new List<UploadedFilesViewModel>();
            using (var ctx = new FlexyboxContext())
            {
                //result = ctx.Query<UploadedFiles>().Where(x=>)
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }

    public class FileManagerViewModel
    {
        public BindingList<UploadedFilesViewModel> Files { get; set; }
        public int CustomerId { get; set; }
        public FileManagerViewModel()
        {
            Files = new BindingList<UploadedFilesViewModel>();
        }
    }
}
