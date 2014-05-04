using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Peach.Core;

namespace Peach.Viewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.Startup += App_Startup;
        }

        void App_Startup(object sender, StartupEventArgs e)
        {
            //Browser.Current.Requesting += Current_Requesting;
            //Browser.Current.Responsed += Current_Responsed;
        }

        void Current_Responsed(object sender, BrowserEventArgs e)
        {
            throw new NotImplementedException();
        }

        void Current_Requesting(object sender, BrowserEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
