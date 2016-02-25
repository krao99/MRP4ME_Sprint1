using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MRP4ME.UIElementsLib
{
    public class DecimalToStringConverter : IValueConverter
    {
        public double EmptyStringValue { get; set; }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;
            else if (value is string)
                return value;
            else if (value is double && (double)value == EmptyStringValue)
                return string.Empty;
            else
                return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string)
            {
                string s = (string)value;
                double numValue;

                if (double.TryParse(s, out numValue))
                    return System.Convert.ToDecimal(s);
                else
                    return EmptyStringValue;
            }
            return value;
        }
    }
}