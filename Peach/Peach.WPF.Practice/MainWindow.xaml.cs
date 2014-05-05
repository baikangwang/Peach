using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Peach.WPF.Practice
{
    using System.Drawing;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Threading;
    using Xceed.Wpf.Toolkit;

    using Peach.Viewer;

    using Color = System.Windows.Media.Color;
    using Image = System.Windows.Controls.Image;

    public delegate void ViewHandler(object sender, object state);

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        StackPanel main=new StackPanel();
        BusyIndicator _loading=new BusyIndicator(){Name = "loading",IsBusy = false};

        public event ViewHandler RowsInitializing;

        public event ViewHandler BeginBusy;

        protected virtual void OnBeginBusy(object state)
        {
            ViewHandler handler = this.BeginBusy;
            if (handler != null)
            {
                handler(this, state);
            }
        }

        public event ViewHandler EndBusy;

        protected virtual void OnEndBusy(object state)
        {
            ViewHandler handler = this.EndBusy;
            if (handler != null)
            {
                handler(this, state);
            }
        }

        protected virtual void OnRowsInitializing(object state)
        {
            ViewHandler handler = this.RowsInitializing;
            if (handler != null)
            {
                handler(this, state);
            }
        }

        public event ViewHandler CellInitializing;

        protected virtual void OnCellInitializing(object state)
        {
            ViewHandler handler = this.CellInitializing;
            if (handler != null)
            {
                handler(this, state);
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.Content = this._loading;
            this.Loaded += MainWindow_Loaded;
            this.ContentRendered += MainWindow_ContentRendered;
            this.RowsInitializing += MainWindow_RowsInitializing;
            this.CellInitializing += MainWindow_CellInitializing;
            this.BeginBusy += MainWindow_BeginBusy;
            this.EndBusy += MainWindow_EndBusy;
        }

        void MainWindow_EndBusy(object sender, object state)
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

        void MainWindow_BeginBusy(object sender, object state)
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

        private void MainWindow_CellInitializing(object sender, object state)
        {
            if (this.Dispatcher.CheckAccess())
            {
                //Group = title, ImageIndex = i, Image = source

                // throw new NotImplementedException();
                Type t = state.GetType();
                string title = t.GetProperty("Group").GetValue(state, null) as string;
                int index = (int)t.GetProperty("ImageIndex").GetValue(state, null);
                ImageSource source = t.GetProperty("Image").GetValue(state, null) as ImageSource;

                GroupBox c = (GroupBox)LogicalTreeHelper.FindLogicalNode(main, title); //(GroupBox)main.FindName(title);

                Image img = (Image)LogicalTreeHelper.FindLogicalNode((c.Content as StackPanel), "img" + (index + 1)); //(Image)(c.Content as StackPanel).FindName("img" + (index + 1));

                if (img.Dispatcher.CheckAccess())
                {
                    img.Source = source;
                }
                else
                {
                    img.Dispatcher.Invoke(
                        DispatcherPriority.Normal,
                        new Action(() => { img.Source = source; }));
                }
            }
            else
            {
                this.Dispatcher.Invoke(
                    DispatcherPriority.Send,
                    new Action<object, object>(this.MainWindow_CellInitializing),
                    sender,
                    state);
            }
        }

        void MainWindow_RowsInitializing(object sender, object state)
        {
            string[] rows = state as string[];

            foreach (string row in rows)
            {
                Control c = this.InitialRow(row);

                this.AddRow(c);
            }
        }

        private void MainWindow_ContentRendered(object sender, EventArgs e)
        {
            Task task = new Task(
                () =>
                    {
                        this.OnBeginBusy(null);

                        string[] rows = new string[4] { "AAA", "BBB", "CCC", "DDD" };

                        Thread.Sleep(3000);

                        this.OnRowsInitializing(rows);

                        BitmapConverter bc = new BitmapConverter();

                        for (int i = 0; i < 4; i++)
                        {
                            string title = rows[i];

                            Task[] tasks = new Task[4];

                            for (int j = 0; j < 4; j++)
                            {
                                int index = j;

                                Task it = new Task(
                                    () =>
                                        {
                                            ImageSource source =
                                                bc.Convert(Properties.Resources.image, null, null, null) as ImageSource;

                                            source.Freeze();

                                            this.OnCellInitializing(
                                                new { Group = title, ImageIndex = index, Image = source });
                                            
                                            Thread.Sleep(3000);
                                        });

                                tasks[j] = it;
                                it.Start();

                                Thread.Sleep(3000);
                            }

                            Task.WaitAll(tasks);
                        }

                        this.OnEndBusy(null);
                    });

            task.Start();

            //task.Wait();
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this._loading.Content = main;
            main.Orientation = Orientation.Vertical;
        }

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

        private Control InitialRow(string title)
        {
            if (this.Dispatcher.CheckAccess())
            {
                GroupBox gb = new GroupBox()
                {
                    Header = title,
                    Name = title
                };
                StackPanel sp = new StackPanel() { Orientation = Orientation.Horizontal };
                Random r = new Random(DateTime.Now.Millisecond);
                sp.Background =
                    new SolidColorBrush(Color.FromRgb(Convert.ToByte(r.Next(0, 225)), Convert.ToByte(r.Next(0, 225)), Convert.ToByte(r.Next(0, 225))));
                BitmapConverter bc = new BitmapConverter();

                Image img1 = new Image()
                {
                    Name = "img1",
                    Source = bc.Convert(Properties.Resources.bg, null, null, null) as BitmapImage,
                    Width = 128,
                    Height = 128,
                    Stretch = Stretch.UniformToFill
                };
                Image img2 = new Image()
                {
                    Name = "img2",
                    Source = bc.Convert(Properties.Resources.bg, null, null, null) as BitmapImage,
                    Width = 128,
                    Height = 128,
                    Stretch = Stretch.UniformToFill
                };
                Image img3 = new Image()
                {
                    Name = "img3",
                    Source = bc.Convert(Properties.Resources.bg, null, null, null) as BitmapImage,
                    Width = 128,
                    Height = 128,
                    Stretch = Stretch.UniformToFill
                };
                Image img4 = new Image()
                {
                    Name = "img4",
                    Source = bc.Convert(Properties.Resources.bg, null, null, null) as BitmapImage,
                    Width = 128,
                    Height = 128,
                    Stretch = Stretch.UniformToFill
                };
                sp.Children.Add(img1);
                sp.Children.Add(img2);
                sp.Children.Add(img3);
                sp.Children.Add(img4);
                gb.Content = sp;

                return gb;
            }
            else
            {
                return (Control)this.Dispatcher.Invoke(
                    DispatcherPriority.Normal, new Func<string, Control>(this.InitialRow), title);
            }
        }
    }
}
