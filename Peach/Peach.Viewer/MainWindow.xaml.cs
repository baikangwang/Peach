using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Threading;
using Peach.Core;
using Peach.Entity;
using Peach.Log;
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
            this.ContentRendered += MainWindow_ContentRendered;
            this.Initialized += MainWindow_Initialized;
            this.Loaded += MainWindow_Loaded;
            this.Closed += MainWindow_Closed;
            this.Unloaded += MainWindow_Unloaded;
            this._view=new HomeView("http://www.imagefap.com");
            this._view.ViewStatusChanged += View_ViewStatusChanged;
            Browser.Current.Requesting += (sender, e) => this.View_ViewStatusChanged(sender, new ViewEventArgs(e.Message));
            Browser.Current.Responsed += (sender, e) => this.View_ViewStatusChanged(sender, new ViewEventArgs(e.Message));
        }

        void MainWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            Logger.Current.Debug("WPF--Unloaded");
        }

        void MainWindow_Closed(object sender, EventArgs e)
        {
            Logger.Current.Debug("WPF--Closed");
        }

        void MainWindow_Initialized(object sender, EventArgs e)
        {
            Logger.Current.Debug("WPF--Initialized");
        }

        void MainWindow_ContentRendered(object sender, EventArgs e)
        {
            Logger.Current.Debug("WPF--ContentRendered");
            Task task = new Task(() =>
                                     {
                                         try
                                         {
                                             this.LoadWebContent();
                                         }
                                         catch (Exception ex)
                                         {
                                             View_ViewStatusChanged(this, new ViewEventArgs(ex.Message));
                                         }
                                     });

            task.Start();

        }

        private void View_ViewStatusChanged(object sender, ViewEventArgs e)
        {
            if (lblMsg.Dispatcher.CheckAccess())
            {
                // The calling thread owns the dispatcher, and hence the UI element
                this.lblMsg.Content = e.Message;
            }
            else
            {
                // Invokation required
                lblMsg.Dispatcher.Invoke(DispatcherPriority.Normal, new ViewEventHandle(View_ViewStatusChanged),sender,e);
            }
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Logger.Current.Debug("WPF--Loaded.");
        }

        private void LoadWebContent()
        {
            this._view.GetView();
            Gallery g = _view.Galleries.FirstOrDefault();
            if (g != null)
            {
                if (group1.Dispatcher.CheckAccess())
                {
                    this.group1.Header = g.Title;
                }
                else
                {
                    group1.Dispatcher.Invoke(new Action(() =>
                                                            {
                                                                this.group1.Header = g.Title;
                                                            }));
                }
                

                IList<Thumbnail> ts = g.Thumbnails.Take(4).ToList();

                if (img1.Dispatcher.CheckAccess())
                {
                    Stream s1 = ts[0].GetContent();
                    if (s1 != null)
                    {
                        img1.Source = BitmapFrame.Create(s1,
                                                         BitmapCreateOptions.None,
                                                         BitmapCacheOption.OnLoad);
                    }
                }
                else
                {
                    img1.Dispatcher.Invoke(new Action(() =>
                    {
                        Stream s1 = ts[0].GetContent();
                        if (s1 != null)
                        {
                            img1.Source = BitmapFrame.Create(s1,
                                                             BitmapCreateOptions.None,
                                                             BitmapCacheOption.OnLoad);
                        }
                    }));
                }

                if (img2.Dispatcher.CheckAccess())
                {
                    Stream s2 = ts[1].GetContent();
                    if (s2 != null)
                    {
                        img2.Source = BitmapFrame.Create(s2,
                                                         BitmapCreateOptions.None,
                                                         BitmapCacheOption.OnLoad);
                    }
                }
                else
                {
                    img2.Dispatcher.Invoke(new Action(() =>
                    {
                        Stream s2 = ts[1].GetContent();
                        if (s2 != null)
                        {
                            img2.Source = BitmapFrame.Create(s2,
                                                             BitmapCreateOptions.None,
                                                             BitmapCacheOption.OnLoad);
                        }
                    }));
                }

                if (img3.Dispatcher.CheckAccess())
                {
                    Stream s3 = ts[2].GetContent();
                    if (s3 != null)
                    {
                        img3.Source = BitmapFrame.Create(s3,
                                                         BitmapCreateOptions.None,
                                                         BitmapCacheOption.OnLoad);
                    }
                }
                else
                {
                    img3.Dispatcher.Invoke(new Action(() =>
                    {
                        Stream s3 = ts[2].GetContent();
                        if (s3 != null)
                        {
                            img3.Source = BitmapFrame.Create(s3,
                                                             BitmapCreateOptions.None,
                                                             BitmapCacheOption.OnLoad);
                        }
                    }));
                }

                if (img4.Dispatcher.CheckAccess())
                {
                    Stream s4 = ts[3].GetContent();
                    if (s4 != null)
                    {
                        img4.Source = BitmapFrame.Create(s4,
                                                         BitmapCreateOptions.None,
                                                         BitmapCacheOption.OnLoad);
                    }
                }
                else
                {
                    img4.Dispatcher.Invoke(new Action(() =>
                    {
                        Stream s4 = ts[3].GetContent();
                        if (s4 != null)
                        {
                            img4.Source = BitmapFrame.Create(s4,
                                                             BitmapCreateOptions.None,
                                                             BitmapCacheOption.OnLoad);
                        }
                    }));
                }
            }
        }
    }
}
