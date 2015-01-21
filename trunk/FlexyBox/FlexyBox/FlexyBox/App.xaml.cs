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
    /// 
    //Lavet af Søren
    public partial class App : Application
    {
        //Expects employeeId customerId
        protected override void OnStartup(StartupEventArgs e)
        {
            //Sæt at den programmet først skal lukke når det bliver bedt om det.
            Current.ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown;
            FlexyDomain.Models.CustomerFlow customer;
            using (var ctx = new FlexyDomain.FlexyboxContext())
            {
                //hent kunden ud, bruges til at se om kunden findes.
                customer = ctx.QueryFromID<FlexyDomain.Models.CustomerFlow>(int.Parse(e.Args[1])).SingleOrDefault();
            }
            if (customer == null)
            {
                //findes kunden ikke, altså at customer = null skal kunden laves.
                bool? dialog = new NewCustomer(int.Parse(e.Args[1]), int.Parse(e.Args[0])).ShowDialog();
                if ( dialog != true)
                    return; 

            }
            //start hovedvinduet
            MainWindow window = new MainWindow(int.Parse(e.Args[0]), int.Parse(e.Args[1]));
            window.Show();
            //sæt at programmet skal lukkes når hovedvinduet bliver lukket.
            Current.ShutdownMode = System.Windows.ShutdownMode.OnMainWindowClose;
        }
    }
}
