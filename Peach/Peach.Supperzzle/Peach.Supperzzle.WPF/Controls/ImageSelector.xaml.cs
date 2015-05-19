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

namespace Pean.Supperzzle.WPF.Pages
{
    using System.IO;

    using Microsoft.Win32;

    using Xceed.Wpf.Toolkit;

    /// <summary>
    /// Interaction logic for ImageSelector.xaml
    /// </summary>
    public partial class ImageSelector : UserControl
    {
        public ImageSelector()
        {
            InitializeComponent();
        }

        public byte[] Image { get; private set; }

        private void btImage_Click(object sender, RoutedEventArgs e)
        {
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
                    try
                    {
                        using (Stream stream = dialog.OpenFile())
                        {
                            byte[] content = new byte[stream.Length];
                            stream.Read(content, 0, content.Length);
                            this.Image = content;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }

                    try
                    {

                        MemoryStream ms = new MemoryStream(this.Image, 0, this.Image.Length);
                        var imageSource = new BitmapImage();
                        imageSource.BeginInit();
                        imageSource.StreamSource = ms;
                        imageSource.EndInit();
                        this.btImage.Background = new ImageBrush(imageSource) { Stretch = Stretch.Fill };
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public string Text
        {
            get
            {
                return this.btImage.Content as string;
            }
            set
            {
                this.btImage.Content = value;
            }
        }

        public void SetBackground(byte[] image)
        {
            this.Image = image;

            try
            {
                try
                {
                    MemoryStream ms = new MemoryStream(image, 0, image.Length);
                    var imageSource = new BitmapImage();
                    imageSource.BeginInit();
                    imageSource.StreamSource = ms;
                    imageSource.EndInit();
                    this.btImage.Background = new ImageBrush(imageSource) { Stretch = Stretch.Fill };
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
