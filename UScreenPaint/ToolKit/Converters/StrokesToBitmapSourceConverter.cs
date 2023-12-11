using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using UScreenPaint.Models;

namespace UScreenPaint.ToolKit.Converters
{
    public class StrokesToBitmapSourceConverter : IValueConverter
    {
        private InkCanvas _canvas = new InkCanvas()
        {
            Background = Brushes.Transparent
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            VectorImage vectorImage = (VectorImage)value;
            var dpi = VisualTreeHelper.GetDpi(App.Current.MainWindow);

            _canvas.Width = vectorImage.Width;
            _canvas.Height = vectorImage.Height;
            _canvas.Strokes = vectorImage.Strokes;
            
            Size size = new Size(vectorImage.Width, vectorImage.Height);
            _canvas.Measure(size);
            _canvas.Arrange(new Rect(size));
            _canvas.UpdateLayout();

            RenderTargetBitmap bmp = new RenderTargetBitmap(vectorImage.Width, vectorImage.Height, dpi.PixelsPerInchX, dpi.PixelsPerInchY, PixelFormats.Pbgra32);
            bmp.Render(_canvas);
            bmp.Freeze();

            return bmp;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
