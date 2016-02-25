using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace MRP4ME.UIElementsLib
{
    public class TabItemBackgroundColourConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //string TabName = (string)value;
            if (value != null && !"HomeViewModel".Equals(value.GetType().Name))
            {
                SolidColorBrush scb = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFE2FEE2"));
                //SolidColorBrush scb = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF0F9D58"));
                return scb;

            }
            else
            {
                SolidColorBrush scb = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFE2FEE2"));
                return scb;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
