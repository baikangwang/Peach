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
using Peach.Entity;
using Peach.View;

namespace Peach.Viewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HomeView _view;
        
        public MainWindow()
        {
            InitializeComponent();
            log4net.Config.XmlConfigurator.Configure();
            this.Loaded += MainWindow_Loaded;
            this._view=new HomeView("http://www.imagefap.com");
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this._view.GetView();
            Gallery g = _view.Galleries.FirstOrDefault();
            if (g != null)
            {
                this.group1.Header = g.Title;

                IList<Thumbnail> ts = g.Thumbnails.Take(4).ToList();

                img1.Source = BitmapFrame.Create(ts[0].GetContent(),
                                      BitmapCreateOptions.None,
                                      BitmapCacheOption.OnLoad);

                img2.Source = BitmapFrame.Create(ts[1].GetContent(),
                                      BitmapCreateOptions.None,
                                      BitmapCacheOption.OnLoad);
                img3.Source = BitmapFrame.Create(ts[2].GetContent(),
                          BitmapCreateOptions.None,
                          BitmapCacheOption.OnLoad);
                img4.Source = BitmapFrame.Create(ts[3].GetContent(),
                          BitmapCreateOptions.None,
                          BitmapCacheOption.OnLoad);
            }
        }
    }
}
