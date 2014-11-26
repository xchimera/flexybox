using FlexyBox.ViewModel;
using System;
using System.IO;
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
using FlexyDomain.Extensions;

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
        public FileManager(CustomerFlowViewModel customer)
        {
            InitializeComponent();
            Model = new FileManagerViewModel();
            Model.Customer = customer;
            Reload();
        }

        private void Reload()
        {
            Model.Files.Clear();
            var result = new List<UploadedFilesViewModel>();
            using (var ctx = new FlexyboxContext())
            {
                result = ctx.Query<UploadedFiles>().Where(x => x.Customer.Id == Model.Customer.Id)
                    .Select(x => new UploadedFilesViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        File = x.File
                    }).ToList();
            }
            Model.Files = result.ToBindingList();
        }

        private bool AddFileToDatabase(string filePath, string fileName)
        {
            byte[] file;
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using(var reader = new BinaryReader(stream))
                {
                    file = reader.ReadBytes((int)stream.Length);
                }
            }

            if (file == null)
                return false;

            var customerEntity = Model.Customer.Entity;
            

            var entity = new UploadedFiles()
            {
                Customer = customerEntity,
                File = file,
                Name = fileName
            };
            bool result;
            using (var ctx = new FlexyboxContext())
            {
                ctx.Entry(customerEntity).State = System.Data.Entity.EntityState.Unchanged;
                result = ctx.SaveEntity<UploadedFiles>(entity);
            }
            
            return result;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = ((sender as ListBox).SelectedItem as UploadedFilesViewModel);
            Stream ReadStream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("file1.xps");
            
        }

        private void AddFile_Click(object sender, RoutedEventArgs e)
        {
            AddFile file = new AddFile();
            file.Owner = this;
            var result = file.ShowDialog();
            if(result == true)
            {
                if (!AddFileToDatabase(file.Model.FileName, file.Model.Name))
                    MessageBox.Show("Der skete en fejl da filen skulle gemmes, prøv igen");
                Reload();
            }

        }

        private void RemoveFile_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    public class FileManagerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private BindingList<UploadedFilesViewModel> _Files;
        public BindingList<UploadedFilesViewModel> Files {
            get
            {
                return _Files;
            }
            set
            {
                _Files = value;
                OnPropertyChanged("Files");
            }
        }
        public CustomerFlowViewModel Customer { get; set; }
        public FileManagerViewModel()
        {
            Files = new BindingList<UploadedFilesViewModel>();
        }
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

    }
}
