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
        #region Fields

        private CancellationToken _globalcancell = new CancellationToken();

        /// <summary>
        /// The _loading.
        /// </summary>
        private readonly BusyIndicator _loading = new BusyIndicator { Name = "loading", IsBusy = false };

        /// <summary>
        /// The main.
        /// </summary>
        private readonly StackPanel main = new StackPanel() { Name = "main", CanVerticallyScroll = true};

        private ScrollViewer _scrollbar = new ScrollViewer()
                                              {
                                                  CanContentScroll = true,
                                                  VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                                              };

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.Content = this._loading;
            this.Loaded += this.MainWindow_Loaded;
            this.ContentRendered += this.MainWindow_ContentRendered;
            this.RowsInitializing += this.MainWindow_RowsInitializing;
            this.CellInitializing += this.MainWindow_CellInitializing;
            this.BeginBusy += this.MainWindow_BeginBusy;
            this.EndBusy += this.MainWindow_EndBusy;
        }

        #endregion

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
        private Control InitialRow(string title,int num)
        {
            if (this.Dispatcher.CheckAccess())
            {
                var gb = new GroupBox { Name = "gb_" + title };

                Button btgb = new Button() { Name = "btgb_" + title, Content = title };
                btgb.Click += button_Click;
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
                    btimg.Click += button_Click;

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

        void button_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            StringBuilder sb=new StringBuilder();
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

                        this.OnEndBusy(null);
                    },
                this._globalcancell);

            task.Start();

            // task.Wait();
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