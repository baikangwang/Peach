using System;
using System.Collections;
using System.Collections.Generic;
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
using Xceed.Wpf.Toolkit;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace Peach.Viewer
{
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields

        private CancellationTokenSource _globalcancell =new CancellationTokenSource();

        private TaskScheduler _ui;

        /// <summary>
        /// The _loading.
        /// </summary>
        private readonly BusyIndicator _loading = new BusyIndicator { Name = "loading", IsBusy = false };

        /// <summary>
        /// The main.
        /// </summary>
        private readonly StackPanel main = new StackPanel() { Name = "main", CanVerticallyScroll = true };

        private ScrollViewer _scrollbar = new ScrollViewer()
        {
            CanContentScroll = true,
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
        };

        #endregion
        
        private HomeView _view;
        
        public MainWindow()
        {
            InitializeComponent();
            log4net.Config.XmlConfigurator.Configure();
            this.Closed += MainWindow_Closed;
            this._view=new HomeView("http://www.imagefap.com");
            //this._view.ViewStatusChanged += View_ViewStatusChanged;
            //Browser.Current.Requesting += (sender, e) => this.View_ViewStatusChanged(sender, new ViewEventArgs(e.Message));
            //Browser.Current.Responsed += (sender, e) => this.View_ViewStatusChanged(sender, new ViewEventArgs(e.Message));

            this.Content = this._loading;
            this.Loaded += this.MainWindow_Loaded;
            this.ContentRendered += this.MainWindow_ContentRendered;
            this.RowsInitializing += this.MainWindow_RowsInitializing;
            this.CellInitializing += this.MainWindow_CellInitializing;
            this.BeginBusy += this.MainWindow_BeginBusy;
            this.EndBusy += this.MainWindow_EndBusy;
            this._ui = TaskScheduler.FromCurrentSynchronizationContext();
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            this._globalcancell.Cancel();
        }

        #region Public Events

        /// <summary>
        /// The begin busy.
        /// </summary>
        public event ViewHandler BeginBusy;

        /// <summary>
        /// The cell initializing.
        /// </summary>
        public event ViewHandler CellInitializing;

        /// <summary>
        /// The end busy.
        /// </summary>
        public event ViewHandler EndBusy;

        /// <summary>
        /// The rows initializing.
        /// </summary>
        public event ViewHandler RowsInitializing;

        #endregion

        #region Methods

        /// <summary>
        /// The on begin busy.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        protected virtual void OnBeginBusy(object state)
        {
            ViewHandler handler = this.BeginBusy;
            if (handler != null)
            {
                handler(this, state);
            }
        }

        /// <summary>
        /// The on cell initializing.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        protected virtual void OnCellInitializing(object state)
        {
            ViewHandler handler = this.CellInitializing;
            if (handler != null)
            {
                handler(this, state);
            }
        }

        /// <summary>
        /// The on end busy.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        protected virtual void OnEndBusy(object state)
        {
            ViewHandler handler = this.EndBusy;
            if (handler != null)
            {
                handler(this, state);
            }
        }

        /// <summary>
        /// The on rows initializing.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        protected virtual void OnRowsInitializing(object state)
        {
            ViewHandler handler = this.RowsInitializing;
            if (handler != null)
            {
                handler(this, state);
            }
        }

        /// <summary>
        /// The add row.
        /// </summary>
        /// <param name="panel">
        /// The panel.
        /// </param>
        private void AddRow(Control panel)
        {
            this.main.ApplyTemplate();
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
        private Control InitialRow(string title,int galleryIndex, int num)
        {
            var gb = new GroupBox { Name = "gb_" + galleryIndex };

            Button btgb = new Button() { Name = "btgb_" + galleryIndex, Content = title };
            btgb.Click += button_Click;
            gb.Header = btgb;

            var sp = new StackPanel {Orientation = Orientation.Horizontal};
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
                btimg.Click += button_Click;

                sp.Children.Add(btimg);
            }

            gb.Content = sp;

            return gb;
        }

        void button_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("Handled -> {0}", e.Handled));
            sb.AppendLine(string.Format("OriginalSource -> {0}", e.OriginalSource ?? "N/A"));
            sb.AppendLine(string.Format("RoutedEvent -> {0}", e.RoutedEvent != null ? e.RoutedEvent.ToString() : "N/A"));
            sb.AppendLine(string.Format("Source -> {0}", e.Source ?? string.Empty));
            MessageBox.Show(this, sb.ToString(), "DEBUG", MessageBoxButton.OK, MessageBoxImage.Information);
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
        private void MainWindow_BeginBusy(object sender, object state)
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
        private void MainWindow_CellInitializing(object sender, object state)
        {
            Type t = state.GetType();
            //var title = t.GetProperty("Group").GetValue(state, null) as string;
            var gindex = (int)t.GetProperty("GalleryIndex").GetValue(state, null);
            var tindex = (int) t.GetProperty("ImageIndex").GetValue(state, null);
            var source = t.GetProperty("Image").GetValue(state, null) as ImageSource;

            var c =
                (GroupBox)
                LogicalTreeHelper.FindLogicalNode(this.main, "gb_" + gindex);
            // (GroupBox)main.FindName(title);

            var bt = (Button) LogicalTreeHelper.FindLogicalNode(c.Content as StackPanel, "btimg" + tindex);
            var img = bt.Content as Image;

            // (Image)(c.Content as StackPanel).FindName("img" + (index + 1));
            img.Source = source;
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
        private void MainWindow_ContentRendered(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
                                      {
                                          Task.Factory.StartNew(() => this.OnBeginBusy(null), CancellationToken.None,
                                                                TaskCreationOptions.None, this._ui);

                                         try
                                         {
                                             this.LoadWebContent();
                                         }
                                         catch (Exception ex)
                                         {
                                             View_ViewStatusChanged(this, new ViewEventArgs(ex.Message));
                                         }

                                         Task.Factory.StartNew(() => this.OnEndBusy(null), CancellationToken.None,
                                                               TaskCreationOptions.None, this._ui);

                                     }, this._globalcancell.Token);
        }

        /// <summary>
        /// The main window_ end busy.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        private void MainWindow_EndBusy(object sender, object state)
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
            this._loading.Content = this._scrollbar;
            this._scrollbar.Content = this.main;
            this.main.Orientation = Orientation.Vertical;
        }

        /// <summary>
        /// The main window_ rows initializing.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        private void MainWindow_RowsInitializing(object sender, object state)
        {
            var rows = state as IList<Gallery>;

            for (int i = 0; i < rows.Count; i++)
            {
                int index = i;

                Gallery target = rows[index];

                Control c = this.InitialRow(target.Title, index, target.Thumbnails.Count);

                this.AddRow(c);
            }
        }

        #endregion

        private void View_ViewStatusChanged(object sender, ViewEventArgs e)
        {
            //if (lblMsg.Dispatcher.CheckAccess())
            //{
            //    // The calling thread owns the dispatcher, and hence the UI element
            //    this.lblMsg.Content = e.Message;
            //}
            //else
            //{
            //    // Invokation required
            //    lblMsg.Dispatcher.Invoke(DispatcherPriority.Normal, new ViewEventHandle(View_ViewStatusChanged),sender,e);
            //}
        }

        private void LoadWebContent()
        {
            this._view.GetView();
            IList<Gallery> gs = _view.Galleries;

            var tinit = Task.Factory.StartNew(() => this.OnRowsInitializing(gs), CancellationToken.None,
                                          TaskCreationOptions.None, this._ui);
            tinit.Wait();

            for (int i = 0; i < gs.Count; i++)
            {
                int gindex = i;
                Gallery g = gs[gindex];
                string title = g.Title;
                int num = g.Thumbnails.Count;

                var tasks = new Task[num];

                for (int j = 0; j < num; j++)
                {
                    int jindex = j;

                    Thumbnail t = g.Thumbnails[jindex];

                    var it = Task.Factory.StartNew(
                        () =>
                            {
                                var stream = t.GetContent();
                                byte[] segment = new byte[1024];
                                int n = stream.Read(segment, 0, segment.Length);
                                MemoryStream ms = new MemoryStream();
                                while (n > 0)
                                {
                                    ms.Write(segment, 0, n);
                                    n = stream.Read(segment, 0, segment.Length);
                                }
                                stream.Close();
                                //ms.Close();
                                ms.Position = 0;
                                var source = BitmapFrame.Create(ms,
                                                                BitmapCreateOptions.None,
                                                                BitmapCacheOption.OnLoad);

                                return source;
                            }, this._globalcancell.Token);

                    it.ContinueWith(task => this.OnCellInitializing(
                        new {Group = title,GalleryIndex=gindex, ImageIndex = jindex, Image = task.Result}), this._ui);

                    tasks[j] = it;
                }

                Task.WaitAll(tasks);
            }
        }
    }
}
