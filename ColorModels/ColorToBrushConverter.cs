using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ColorModels
{
    //[ValueConversion(typeof(string), typeof(double))]
    public class ColorToBrushConverter : IValueConverter
    {       
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value != null)
            {               
                return new SolidColorBrush(Color.FromRgb(((Color)value).R, ((Color)value).G, ((Color)value).B));
            }
            else {
                return new SolidColorBrush(Color.FromRgb(0,255,0));
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

       
    }
}
