using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace UScreenPaint.ToolKit.Converters
{
    public class InkCanvasEditingModeToCursorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((InkCanvasEditingMode)value)
            {
                case InkCanvasEditingMode.Ink:
                case InkCanvasEditingMode.GestureOnly:
                    return Cursors.Pen;

                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
