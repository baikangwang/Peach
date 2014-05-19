using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
using Peach.Core;
using Peach.Entity;
using Peach.Log;
using Peach.View;
using Peach.Viewer.EventHandler;
using Xceed.Wpf.Toolkit;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace Peach.Viewer
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : BasePage
    {
        #region Fields

        private TaskScheduler _ui;

        /// <summary>
        /// The _loading.
        /// </summary>
        private readonly BusyIndicator _loading = new BusyIndicator { Name = "loading", IsBusy = false };

        /// <summary>
        /// The main.
        /// </summary>
        private readonly StackPanel main = new StackPanel() { Name = "main", CanVerticallyScroll = true };

        private Button _retry = new Button() { Name = "btRetry" };

        private ScrollViewer _scrollbar = new ScrollViewer()
        {
            CanContentScroll = true,
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
        };

        private HomeView _view;

        #endregion
        
        public HomePage()
        {
            InitializeComponent();

            this._view = new HomeView("http://www.imagefap.com");
            //this._view.ViewGalleryProcessed += HomeView_GalleryProcessed;
            this._view.ViewStatusChanged += HomeView_StatusChanged;
            this.Loaded += this.MainWindow_Loaded;
        }

        private void HomeView_StatusChanged(object sender, ParserStatusEventArgs e)
        {
            Logger.Current.Debug(e.Message);
            Task.Factory.StartNew(() =>
            {
                if (this._loading.IsBusy)
                {
                    this._loading.BusyContent = e.Message;
                }
            }, CancellationToken.None, TaskCreationOptions.None, _ui);

        }

        #region NOT USE
        private void HomeView_GalleryProcessed(object sender, ParserProcessEventArgs args)
        {
            Task.Factory.StartNew(() =>
            {
                Control row = InitialRow(args.Gallery, args.Index,
                                         args.Gallery.Thumbnails.Count);

                this.AddRow(row);
                return row;
            }, this.PageCancellationToken.Token, TaskCreationOptions.None, _ui);
        }

        private void RefreshDownloadProgress(Control row, int thumindex, long totalLength, long currentLength)
        {
            var c = (GroupBox)row;

            var busy = (BusyIndicator)LogicalTreeHelper.FindLogicalNode(c.Content as StackPanel, "busy_" + thumindex);
            Label lbl = busy.BusyContent as Label;
            lbl.Content = string.Format("Loading {0}", totalLength == 0
                                                           ? "N/A"
                                                           : Math.Round((double)currentLength * 100 / (double)totalLength, 2) + "%");
        }
        #endregion

        #region Methods

        /// <summary>
        /// The add row.
        /// </summary>
        /// <param name="panel">
        /// The panel.
        /// </param>
        private void AddRow(Control panel)
        {
            this.main.Children.Add(panel);
        }

        /// <summary>
        /// The initial row.
        /// </summary>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <returns>
        /// The <see cref="Control"/>.
        /// </returns>
        private Control InitialRow(Gallery gallery, int galleryIndex, int num)
        {
            string title = gallery.Title;
            var gb = new GroupBox { Name = "gb_" + galleryIndex };

            Button btgb = new Button() { Name = "btgb_" + galleryIndex, Content = title };
            btgb.Click += btGallery_Click;
            btgb.Tag = new GalleryEventArgs(galleryIndex, gallery);
            gb.Header = btgb;

            var sp = new StackPanel { Orientation = Orientation.Horizontal };
            var r = new Random(DateTime.Now.Millisecond);
            sp.Background =
                new SolidColorBrush(
                    Color.FromRgb(
                        Convert.ToByte(r.Next(0, 225)),
                        Convert.ToByte(r.Next(0, 225)),
                        Convert.ToByte(r.Next(0, 225))));
            var bc = new BitmapConverter();

            ImageSource source = bc.Convert(Properties.Resources.bg, null, null, null) as BitmapImage;
            source.Freeze();

            for (int i = 0; i < num; i++)
            {
                var thumImage = InitThumImage(i, source);

                sp.Children.Add(thumImage);
            }

            gb.Content = sp;

            return gb;
        }

        private Control InitThumImage(int i, ImageSource source)
        {
            BusyIndicator busy = new BusyIndicator()
            {
                Name = "busy_" + i,
                IsBusy = true
            };
            //Label lbl=new Label()
            //              {
            //                  Name = "lbl_"+i,
            //                  Content = "Loading 0%"
            //              };
            //busy.BusyContent = lbl;


            var btimg = new Button()
            {
                Name = "btimg" + i // ,
                // Style = this.FindResource("NoChromeButton") as Style
            };

            var img = new Image
            {
                Name = "img" + i,
                Source = source,
                Width = 128,
                Height = 128,
                Stretch = Stretch.UniformToFill
            };

            btimg.Content = img;
            btimg.Click += btImage_Click;
            busy.Content = btimg;
            return busy;
        }

        void btImage_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            //StringBuilder sb = new StringBuilder();
            //sb.AppendLine(string.Format("Handled -> {0}", e.Handled));
            //sb.AppendLine(string.Format("OriginalSource -> {0}", e.OriginalSource ?? "N/A"));
            //sb.AppendLine(string.Format("RoutedEvent -> {0}", e.RoutedEvent != null ? e.RoutedEvent.ToString() : "N/A"));
            //sb.AppendLine(string.Format("Source -> {0}", e.Source ?? string.Empty));
            //MessageBox.Show(sb.ToString(), "DEBUG", MessageBoxButton.OK, MessageBoxImage.Information);
            Button bt = sender as Button;
            ImageEventArgs imgEventArgs = bt.Tag as ImageEventArgs;
            if (imgEventArgs != null)
            {
                this.OnImageClicked(imgEventArgs);
            }
            else
            {
                //todo log
            }
        }

        void btGallery_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            //StringBuilder sb = new StringBuilder();
            //sb.AppendLine(string.Format("Handled -> {0}", e.Handled));
            //sb.AppendLine(string.Format("OriginalSource -> {0}", e.OriginalSource ?? "N/A"));
            //sb.AppendLine(string.Format("RoutedEvent -> {0}", e.RoutedEvent != null ? e.RoutedEvent.ToString() : "N/A"));
            //sb.AppendLine(string.Format("Source -> {0}", e.Source ?? string.Empty));
            //MessageBox.Show(sb.ToString(), "DEBUG", MessageBoxButton.OK, MessageBoxImage.Information);
            Button bt = sender as Button;
            GalleryEventArgs galleryEventArgs = bt.Tag as GalleryEventArgs;
            if (galleryEventArgs != null)
            {
                this.OnGalleryClicked(galleryEventArgs);
            }
            else
            {
                //todo log
            }
        }

        /// <summary>
        /// The main window_ begin busy.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        private void BeginBusy()
        {
            if (!this._loading.IsBusy)
            {
                this._loading.IsBusy = true;
            }
        }

        /// <summary>
        /// The main window_ cell initializing.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        private void LoadThumImage(Control row,IThumbnail thum, int thumindex, ImageSource thumContent)
        {
            var c = (GroupBox)row;

            var busy = (BusyIndicator)LogicalTreeHelper.FindLogicalNode(c.Content as StackPanel, "busy_" + thumindex);
            busy.IsBusy = false;

            if (thumContent != null)
            {
                var bt = busy.Content as Button;
                bt.Tag = new ImageEventArgs(thumindex, thum);
                var img = bt.Content as Image;
                img.Source = thumContent;
            }
        }

        /// <summary>
        /// The main window_ content rendered.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>

        /// <summary>
        /// The main window_ end busy.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        private void EndBusy()
        {
            if (this._loading.IsBusy)
            {
                this._loading.IsBusy = false;
            }
        }

        /// <summary>
        /// The main window_ loaded.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.IsCompleted)
            {
                return;
            }
            
            this.Content = this._scrollbar;
            this._scrollbar.Content = this._loading;
            this._loading.Content = this.main;
            this.main.Orientation = Orientation.Vertical;
            this.main.Children.Add(_retry);
            this._retry.Visibility = Visibility.Hidden;
            this._retry.Click += (btretry, args) =>
            {
                Button bt = btretry as Button;
                bt.Visibility = Visibility.Hidden;
                Task.Factory.StartNew(this.LoadHomeView, this.PageCancellationToken.Token);
            };
            this._ui = TaskScheduler.FromCurrentSynchronizationContext();
            Task.Factory.StartNew(this.LoadHomeView, this.PageCancellationToken.Token);
            this.IsCompleted = true;
            Task.Factory.StartNew(this.LoadThumContent, this.PageCancellationToken.Token);
        }

        private void LoadThumContent()
        {
            this.BasicControlReady.WaitOne();

            IList<Gallery> gs = this._view.Galleries;

            for (int i = 0; i < gs.Count; i++)
            {
                int gindex = i;
                Gallery g = gs[gindex];

                int num = g.Thumbnails.Count;

                for (int j = 0; j < num; j++)
                {
                    int tindex = j;

                    IThumbnail thum = g.Thumbnails[tindex];

                    //(thum as Thumbnail).ImageDownloading += (img, args) => Task.Factory.StartNew(
                    //    () =>
                    //    this.RefreshDownloadProgress(row, tindex, args.TotalLength, args.CurrentLength),
                    //    this.this.PageCancellationToken.Token, TaskCreationOptions.None, this._ui);

                    var t = Task.Factory.StartNew(() => { thum.Ready.WaitOne(); }, this.PageCancellationToken.Token);
                    
                    t.ContinueWith(tthum =>
                                       {
                                           Stream content = thum.GetContent();
                                           ImageSource source = null;
                                           if (content != null)
                                           {
                                               source =
                                                   BitmapFrame.Create(thum.GetContent(),
                                                                      BitmapCreateOptions
                                                                          .None,
                                                                      BitmapCacheOption
                                                                          .OnLoad);
                                               source.Freeze();
                                           }

                                           //Thread.Sleep(100);

                                           Control row =
                                               LogicalTreeHelper.FindLogicalNode(this.main as StackPanel,
                                                                                 "gb_" + gindex) as GroupBox;
                                           this.LoadThumImage(row,thum, tindex, source);

                                       }, this.PageCancellationToken.Token,
                                   TaskContinuationOptions.None, this._ui);
                }
            }
        }

        private void LoadHomeView()
        {
            try
            {
                Task.Factory.StartNew(this.BeginBusy, CancellationToken.None,
                                      TaskCreationOptions.None, this._ui);

                this._view.GetView();
                Task.Factory.StartNew(() =>
                {
                    List<Task> tasks = new List<Task>();
                    for (int i = 0; i < this._view.Galleries.Count; i++)
                    {
                        int gindex = i;
                        Gallery gallery = this._view.Galleries[i];
                        var trow = Task.Factory.StartNew(() =>
                        {
                            Control c = InitialRow(gallery, gindex, gallery.Thumbnails.Count);
                            this.AddRow(c);
                        }, this.PageCancellationToken.Token, TaskCreationOptions.None, _ui);
                        tasks.Add(trow);
                    }
                    Task.WaitAll(tasks.ToArray());
                    BasicControlReady.Set();

                }, this.PageCancellationToken.Token);

                Task.Factory.StartNew(() =>
                {
                    foreach (Gallery gallery in this._view.Galleries)
                    {
                        foreach (IThumbnail thum in gallery.Thumbnails)
                        {
                            Task.Factory.StartNew(thum.Load, this.PageCancellationToken.Token);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
                Task.Factory.StartNew(() =>
                {
                    this._retry.Visibility = Visibility.Visible;
                    this._retry.Content =
                        string.Format("Home Page Load fails. Retry later please.");
                }, CancellationToken.None,
                                      TaskCreationOptions.None, this._ui);
            }
            finally
            {
                Task.Factory.StartNew(this.EndBusy, CancellationToken.None,
                                      TaskCreationOptions.None, this._ui);
            }
        }

        #endregion
    }
}
