using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FlexyBox
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //Expects employeeId customerId
        protected override void OnStartup(StartupEventArgs e)
        {
            new MainWindow(int.Parse(e.Args[0]), int.Parse(e.Args[1])).Show();
        }
    }
}
