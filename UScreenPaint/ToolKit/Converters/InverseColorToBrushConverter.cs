using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace UScreenPaint.ToolKit.Converters
{
    public class InverseColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color clr = (Color)value;
            clr.R = (byte)(byte.MaxValue - clr.R);
            clr.G = (byte)(byte.MaxValue - clr.G);
            clr.B = (byte)(byte.MaxValue - clr.B);

            return new SolidColorBrush(clr);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
