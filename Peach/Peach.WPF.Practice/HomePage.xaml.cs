using System;
using System.Collections.Generic;
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
using System.Windows.Threading;
using Peach.Viewer;
using Xceed.Wpf.Toolkit;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace Peach.WPF.Practice
{
    public class GalleryEventArgs:EventArgs
    {
        public int Index { get; private set; }
        public string Gallery { get; private set; }

        public GalleryEventArgs(int index, string gallery)
        {
            this.Index = index;
            this.Gallery = gallery;
        }
    }

    public class ThumbnailEventArgs:EventArgs
    {
        public int Index { get; private set; }
        public string Thumbnail { get; private set; }

        public ThumbnailEventArgs(int index, string thumbnail)
        {
            this.Index = index;
            this.Thumbnail = thumbnail;
        }
    }

    public delegate void GalleryEventHandler(object sender, GalleryEventArgs e);

    public delegate void ThumbnailEventHandler(object sender, ThumbnailEventArgs e);
    
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : UserControl
    {
        #region Fields

        private CancellationToken _globalcancell = new CancellationToken();

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
        
        
        public HomePage()
        {
            InitializeComponent();
            this.Content = this._loading;
            this.Loaded += this.MainWindow_Loaded;
            this.RowsInitializing += this.MainWindow_RowsInitializing;
            this.CellInitializing += this.MainWindow_CellInitializing;
            this.BeginBusy += this.MainWindow_BeginBusy;
            this.EndBusy += this.MainWindow_EndBusy;
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

        public event GalleryEventHandler GalleryClicked;
        public event ThumbnailEventHandler ThumbnailClicked;

        #endregion

        #region Methods

        protected virtual void OnGalleryClicked(GalleryEventArgs e)
        {
            GalleryEventHandler handler = GalleryClicked;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnThumbnailClicked(ThumbnailEventArgs e)
        {
            ThumbnailEventHandler handler = ThumbnailClicked;
            if (handler != null) handler(this, e);
        }

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
            if (!this.main.Dispatcher.CheckAccess())
            {
                this.main.Dispatcher.Invoke(DispatcherPriority.Normal, new Action<Control>(this.AddRow), panel);
            }
            else
            {
                this.main.ApplyTemplate();
                this.main.Children.Add(panel);
            }
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
        private Control InitialRow(string title, int num)
        {
            if (this.Dispatcher.CheckAccess())
            {
                var gb = new GroupBox { Name = "gb_" + title };

                Button btgb = new Button() { Name = "btgb_" + title, Content = title };
                btgb.Click += btGallery_Click;
                btgb.Tag = title;
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
                    var btimg = new Button()
                    {
                        Name = "btimg" + i// ,
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
                    btimg.Click += btThum_Click;
                    btimg.Tag = i;

                    sp.Children.Add(btimg);
                }

                gb.Content = sp;

                return gb;
            }
            else
            {
                return
                    (Control)
                    this.Dispatcher.Invoke(
                        DispatcherPriority.Normal, new Func<string, int, Control>(this.InitialRow), title, num);
            }
        }

        void btThum_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("Handled -> {0}", e.Handled));
            sb.AppendLine(string.Format("OriginalSource -> {0}", e.OriginalSource ?? "N/A"));
            sb.AppendLine(string.Format("RoutedEvent -> {0}", e.RoutedEvent != null ? e.RoutedEvent.ToString() : "N/A"));
            sb.AppendLine(string.Format("Source -> {0}", e.Source ?? string.Empty));
            MessageBox.Show(sb.ToString(), "DEBUG", MessageBoxButton.OK, MessageBoxImage.Information);
            Button bt = sender as Button;
            object thum = bt.Tag;
            this.OnThumbnailClicked(new ThumbnailEventArgs(0, thum==null?"N/A":thum.ToString()));
        }

        void btGallery_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("Handled -> {0}", e.Handled));
            sb.AppendLine(string.Format("OriginalSource -> {0}", e.OriginalSource ?? "N/A"));
            sb.AppendLine(string.Format("RoutedEvent -> {0}", e.RoutedEvent != null ? e.RoutedEvent.ToString() : "N/A"));
            sb.AppendLine(string.Format("Source -> {0}", e.Source ?? string.Empty));
            MessageBox.Show(sb.ToString(), "DEBUG", MessageBoxButton.OK, MessageBoxImage.Information);
            Button bt = sender as Button;
            object gallery = bt.Tag;
            this.OnGalleryClicked(new GalleryEventArgs(0, gallery == null ? "N/A" : gallery.ToString()));
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
            if (this._loading.Dispatcher.CheckAccess())
            {
                if (!this._loading.IsBusy)
                {
                    this._loading.IsBusy = true;
                }
            }
            else
            {
                this._loading.Dispatcher.Invoke(
                    DispatcherPriority.Normal, new Action<object, object>(this.MainWindow_BeginBusy), sender, state);
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
            if (this.Dispatcher.CheckAccess())
            {
                // Group = title, ImageIndex = i, Image = source

                // throw new NotImplementedException();
                Type t = state.GetType();
                var title = t.GetProperty("Group").GetValue(state, null) as string;
                var index = (int)t.GetProperty("ImageIndex").GetValue(state, null);
                var source = t.GetProperty("Image").GetValue(state, null) as ImageSource;

                var c = (GroupBox)LogicalTreeHelper.FindLogicalNode(this.main, "gb_" + title); // (GroupBox)main.FindName(title);

                var bt = (Button)LogicalTreeHelper.FindLogicalNode(c.Content as StackPanel, "btimg" + index);
                var img = bt.Content as Image;

                // (Image)(c.Content as StackPanel).FindName("img" + (index + 1));
                if (img.Dispatcher.CheckAccess())
                {
                    img.Source = source;
                }
                else
                {
                    img.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { img.Source = source; }));
                }
            }
            else
            {
                this.Dispatcher.Invoke(
                    DispatcherPriority.Send, new Action<object, object>(this.MainWindow_CellInitializing), sender, state);
            }
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
            if (this._loading.Dispatcher.CheckAccess())
            {
                if (this._loading.IsBusy)
                {
                    this._loading.IsBusy = false;
                }
            }
            else
            {
                this._loading.Dispatcher.Invoke(
                    DispatcherPriority.Normal, new Action<object, object>(this.MainWindow_EndBusy), sender, state);
            }
        }

        private bool _isCompleted = false;

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
            if (this._isCompleted)
            {
                MessageBox.Show("Home.IsCompleted");
                return;
            }
            MessageBox.Show("Home.Loaded");

            this._loading.Content = this._scrollbar;
            this._scrollbar.Content = this.main;
            this.main.Orientation = Orientation.Vertical;

            var task = new Task(
                () =>
                {
                    this.OnBeginBusy(null);

                    var rows = new[]
                                       {
                                           "AAA:4", "BBB:4", "CCC:4", "DDD:4", "EEE:4", "FFF:4", "GGG:4", "HHH:4", "III:4"
                                           , "JJJ:4", "KKK:4", "LLL:4", "MMM:4", "NNN:4", "OOO:4"
                                       };

                    Thread.Sleep(100);

                    this.OnRowsInitializing(rows);

                    var bc = new BitmapConverter();

                    for (int i = 0; i < rows.Length; i++)
                    {
                        string[] parts = rows[i].Split(new string[] { ":" }, StringSplitOptions.None);
                        string title = parts[0];
                        int num = int.Parse(parts[1]);

                        var tasks = new Task[num];

                        for (int j = 0; j < num; j++)
                        {
                            int index = j;

                            var it = new Task(
                                () =>
                                {
                                    var source =
                                        bc.Convert(Properties.Resources.image, null, null, null) as ImageSource;

                                    source.Freeze();

                                    this.OnCellInitializing(
                                        new { Group = title, ImageIndex = index, Image = source });

                                    Thread.Sleep(100);
                                });

                            tasks[j] = it;
                            it.Start();

                            Thread.Sleep(100);
                        }

                        Task.WaitAll(tasks);
                    }

                    this._isCompleted = true;

                    this.OnEndBusy(null);
                },
                this._globalcancell);

            task.Start();
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
            var rows = state as string[];

            foreach (string row in rows)
            {
                string[] parts = row.Split(new string[] { ":" }, StringSplitOptions.None);

                Control c = this.InitialRow(parts[0], int.Parse(parts[1]));

                this.AddRow(c);
            }
        }

        #endregion
    }
}
