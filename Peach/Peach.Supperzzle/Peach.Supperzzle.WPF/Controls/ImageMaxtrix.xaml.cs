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

namespace Pean.Supperzzle.WPF.Controls
{
    using System.Drawing;
    using System.IO;

    using Microsoft.Win32;

    using Xceed.Wpf.Toolkit;

    using Image = System.Windows.Controls.Image;

    /// <summary>
    /// Interaction logic for ImageMaxtrix.xaml
    /// </summary>
    public partial class ImageMaxtrix : UserControl
    {
        private Image[,] _imgControls;

        private int _columns;

        private int _rows;

        private IList<byte[]> _images; 

        public ImageMaxtrix()
        {
            this.InitializeComponent();
            this.panel.ShowGridLines = true;
            this._images=new List<byte[]>();
        }

        public void Init(int columns, int rows)
        {
            this._imgControls = new Image[columns,rows];
            this._columns = columns;
            this._rows = rows;
            this.panel.Children.Clear();
            this.panel.ColumnDefinitions.Clear();
            this.panel.RowDefinitions.Clear();

            for (int i = 0; i < columns; i++)
            {
                this.panel.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < rows; i++)
            {
                this.panel.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    Image img = new Image(); //{ Width = 160, Height = 120 };
                    img.Stretch=Stretch.Fill;
                    Grid.SetColumn(img, i);
                    Grid.SetRow(img, j);
                    this.panel.Children.Add(img);
                    this._imgControls[i, j] = img;
                }
            }
        }

        public void Init(int columns, int rows, IList<byte[]> images)
        {
            this._columns = columns;
            this._rows = rows;
            this._images = images;
            this._imgControls = new Image[columns, rows];
            this.panel.Children.Clear();
            this.panel.ColumnDefinitions.Clear();
            this.panel.RowDefinitions.Clear();

            for (int i = 0; i < columns; i++)
            {
                this.panel.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < rows; i++)
            {
                this.panel.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    Image img = new Image();// { Width = 160, Height = 120 };

                    byte[] image = images[i * rows + j];

                    MemoryStream ms = new MemoryStream(image, 0, image.Length);

                    var imageSource = new BitmapImage();
                    imageSource.BeginInit();
                    imageSource.StreamSource = ms;
                    imageSource.EndInit();
                    img.Source = imageSource;
                    img.Stretch = Stretch.Fill;

                    Grid.SetColumn(img, i);
                    Grid.SetRow(img, j);
                    this.panel.Children.Add(img);

                    this._imgControls[i, j] = img;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // index puzzle background
            // -----------------
            // | 0 | 1 | 2 | 3 |
            // -----------------
            // | 4 | 5 | 6 | 7 |
            // -----------------
            // | 8 | 9 | 10| 11|
            // -----------------

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;
            dialog.Filter = "All Files (*.*)|*.*|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            bool? selected = dialog.ShowDialog();
            if (selected.HasValue && selected.Value)
            {
                try
                {
                    IList<byte[]> cache=new List<byte[]>();

                    byte[] orignal;
                    using (Stream stream = dialog.OpenFile())
                    {
                        orignal=new byte[stream.Length];
                        stream.Read(orignal, 0, orignal.Length);
                    }

                    using (MemoryStream stream=new MemoryStream(orignal,0,orignal.Length))
                    {
                        using (System.Drawing.Image one = System.Drawing.Image.FromStream(stream))
                        {
#if DEBUG
                            one.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "one.jpg"));
#endif
                                double ow = one.Size.Width;
                                double oh = one.Size.Height;

                                int sw = (int)(ow / this._columns);
                                int sh = (int)(oh / this._rows);

                                for (int i = 0; i < this._columns; i++)
                                {
                                    for (int j = 0; j < this._rows; j++)
                                    {
                                        using (Bitmap img = new Bitmap(sw, sh))
                                        {
                                            using (Graphics g = Graphics.FromImage(img))
                                            {
                                                g.DrawImage(one,new Rectangle(0, 0, sw, sh),new Rectangle(i * sw, j * sh, sw, sh),GraphicsUnit.Pixel);
                                            }
#if DEBUG
                                            img.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, (i*this._rows+j) + ".jpg"));
#endif

                                            MemoryStream ms1 = new MemoryStream();
                                            img.Save(ms1, one.RawFormat);
                                            byte[] content = ms1.GetBuffer();
                                            cache.Add(content);

                                            ms1.Position = 0;
                                            var imageSource = new BitmapImage();
                                            imageSource.BeginInit();
                                            imageSource.StreamSource = ms1;
                                            imageSource.EndInit();
                                            this._imgControls[i, j].Source = imageSource;
                                            this._imgControls[i, j].Stretch = Stretch.Fill;
                                        }
                                    }
                                }
                        }
                    }

                    this._images = cache;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public IList<byte[]> Images
        {
            get
            {
                return this._images;
            }
        }
    }
}
