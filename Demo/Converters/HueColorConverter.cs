using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using DominantColor;
using Color = System.Drawing.Color;

namespace Demo.Converters
{
    public class HueColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int val = (int) value;
            Color color =  ColorUtils.ColorFromHSV(val, 1, 1);
            System.Windows.Media.Color convertedColor = Utils.ConvertDrawingColor(color);
            return new SolidColorBrush(convertedColor);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
