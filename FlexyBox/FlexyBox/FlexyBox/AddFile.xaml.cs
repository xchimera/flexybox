using Microsoft.Win32;
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

namespace FlexyBox
{
    /// <summary>
    /// Interaction logic for AddFile.xaml
    /// </summary>
    public partial class AddFile : Window
    {
        public AddFileViewModel Model
        {
            get { return DataContext as AddFileViewModel; }
            set { DataContext = value; }
        }
        public AddFile()
        {
            InitializeComponent();
            Model = new AddFileViewModel();
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            bool? result = fileDialog.ShowDialog();

            if(result == true)
            {
                Model.FileName = fileDialog.FileName;
            }
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            Close();
        }
    }
    public class AddFileViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _FileName;
        public string Name { get; set; }
        public string FileName {
            get
            {
                return _FileName;
            }
            set
            {
                _FileName = value;
                OnPropertyChanged("FileName");
            }
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
