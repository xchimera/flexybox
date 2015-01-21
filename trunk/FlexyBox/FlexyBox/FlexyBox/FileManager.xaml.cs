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
    /// 
    //Lavet af Søren
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
            //fjern alle elementer fra listen så vi ikke får dobbelt liste
            Model.Files.Clear();
            var result = new List<CustomerFileViewModel>();
            using (var ctx = new FlexyboxContext())
            {
                //hent filerne som er tilknyttet kunden og lav dem om til en view model
                result = ctx.Query<CustomerFile>().Where(x => x.Customer.Id == Model.Customer.Id)
                    .Select(x => new CustomerFileViewModel()
                    {
                        Id = x.Id,
                        
                        File = x.File
                    }).ToList();
            }
            //sæt modellens fil liste til resultet
            Model.Files = result.ToBindingList();
        }

        private bool AddFileToDatabase(string filePath, string fileName)
        {
            byte[] file;
            //skab en stream der lukker efter brug
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                //skab en binary reader der bliver lukket efter brug
                using(var reader = new BinaryReader(stream))
                {
                    //hent den pågældende fil fra disken ind i rammene
                    file = reader.ReadBytes((int)stream.Length);
                }
            }
            //hvis filen ikke kunne læses
            if (file == null)
                return false;

            var customerEntity = Model.Customer.Entity;
            
            //skab et nyt CustomerFile objekt men kunden og filen
            var entity = new CustomerFile()
            {
                Customer = customerEntity,
                File = file,
                Name = fileName
            };
            bool result;
            using (var ctx = new FlexyboxContext())
            {
                //fortæl at kundens entitet ikke er ny og der derfor ikke skal skabes en ny i databasen
                ctx.Entry(customerEntity).State = System.Data.Entity.EntityState.Unchanged;
                //gem filen
                result = ctx.SaveEntity<CustomerFile>(entity);
            }
            
            return result;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ikke lavet færdig endnu
            var item = ((sender as ListBox).SelectedItem as CustomerFile);
            Stream ReadStream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("file1.xps");
            
        }

        private void AddFile_Click(object sender, RoutedEventArgs e)
        {
            AddFile file = new AddFile();
            file.Owner = this;
            //hvis dialogen til at uploade en fil
            var result = file.ShowDialog();
            if(result == true)
            {
                //blev der trykket ok med en fil indskrevet skal filen gemmes
                if (!AddFileToDatabase(file.Model.FileName, file.Model.Name))
                    MessageBox.Show("Der skete en fejl da filen skulle gemmes, prøv igen");
                //opdater viewet
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
        private BindingList<CustomerFileViewModel> _Files;
        public BindingList<CustomerFileViewModel> Files
        {
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
            Files = new BindingList<CustomerFileViewModel>();
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
