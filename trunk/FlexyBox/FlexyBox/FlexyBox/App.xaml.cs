using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
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
            if (Debugger.IsAttached)
                HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();
            FlexyDomain.Models.CustomerFlow customer;
            using (var ctx = new FlexyDomain.FlexyboxContext())
            {
                customer = ctx.QueryFromID<FlexyDomain.Models.CustomerFlow>(int.Parse(e.Args[1])).SingleOrDefault();
            }
            if (customer == null)
            {
                if (new NewCustomer(int.Parse(e.Args[1]), int.Parse(e.Args[0])).ShowDialog() != true)
                    return; 

            }
                
            new MainWindow(int.Parse(e.Args[0]), int.Parse(e.Args[1])).Show();
        }
    }
}
