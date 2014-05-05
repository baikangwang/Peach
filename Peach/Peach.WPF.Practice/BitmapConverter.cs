using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Peach.Viewer
{
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.IO;
    using System.Windows.Data;
    using System.Windows.Media.Imaging;

    public class BitmapConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            MemoryStream ms=new MemoryStream();
            ((System.Drawing.Bitmap)value).Save(ms, ImageFormat.Bmp);
            BitmapImage image=new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();

            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
