using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
using System.Windows.Threading;
using Peach.Core;
using Peach.Entity;
using Peach.Log;
using Peach.View;
using Peach.Viewer.EventHandler;
using Xceed.Wpf.Toolkit;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;
using Path = System.IO.Path;

namespace Peach.Viewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TabControl _container = new TabControl();
        
        public MainWindow()
        {
            InitializeComponent();
            log4net.Config.XmlConfigurator.Configure();
            this.Closed += MainWindow_Closed;
            this.Content = this._container;
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            foreach (TabItem tabItem in this._container.Items)
            {
                BasePage page = tabItem.Content as BasePage;
                if (page != null)
                {
                    page.PageCancellationToken.Cancel();
                }
            }
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            TabItem home = new TabItem()
            {
                Name = "tbitm_home",
                Header = "Home"
            };

            HomePage homepage = new HomePage()
            {
                Name = "page_home"
            };
            homepage.GalleryClicked += MainWindow_GalleryClicked;
            homepage.ImageClicked += MainWindow_ImageClicked;
            home.Content = homepage;

            _container.Items.Add(home);
            _container.SelectedItem = home;
        }

        void MainWindow_ImageClicked(object sender, ImageEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void MainWindow_GalleryClicked(object sender, GalleryEventArgs e)
        {
            string title = e.Gallery.Title;

            TabItem gallery = new TabItem()
            {
                Name = "tbitem_g_" + e.Index,
                Header = title
            };

            GalleryPage page = new GalleryPage()
            {
                Name = "page_g_" + e.Index
            };
            page.GalleryClicked += MainWindow_GalleryClicked;
            page.ImageClicked += MainWindow_ImageClicked;
            gallery.Content = page;
            _container.Items.Add(gallery);
            _container.SelectedItem = gallery;
        }
    }
}
