// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Orange">
//   
// </copyright>
// <summary>
//   The view handler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Peach.WPF.Practice
{
    using System;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Threading;

    using Peach.Viewer;

    using Xceed.Wpf.Toolkit;

    using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

    /// <summary>
    /// The view handler.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="state">
    /// The state.
    /// </param>
    public delegate void ViewHandler(object sender, object state);

    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window 
    {
        private TabControl _container=new TabControl();

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.Content = this._container;
            this.Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
           TabItem home=new TabItem()
                            {
                                Name = "tbitem_home",
                                Header = "Home"
                            };

            HomePage homepage=new HomePage()
                                  {
                                      Name = "page_home"
                                  };
            homepage.GalleryClicked += homepage_GalleryClicked;
            homepage.ThumbnailClicked += homepage_ThumbnailClicked;
                home.Content = homepage;

            _container.Items.Add(home);
            _container.SelectedItem = home;
        }

        void homepage_ThumbnailClicked(object sender, ThumbnailEventArgs e)
        {
            throw new NotImplementedException();
        }

        void homepage_GalleryClicked(object sender, GalleryEventArgs e)
        {
            string title = e.Gallery;
            
            TabItem gallery = new TabItem()
            {
                Name = "tbitem_g_"+title,
                Header = title
            };

            GalleryPage page = new GalleryPage()
            {
                Name = "page_g_"+title
            };
            gallery.Content = page;
            _container.Items.Add(gallery);
            _container.SelectedItem = gallery;
        }

        #endregion
    }
}