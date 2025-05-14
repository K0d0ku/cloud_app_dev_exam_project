using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace cloud_app_dev_exam_project.Converters
{
    public class BoolToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "Deselect" : "Select";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString() == "Select" ? false : true;
        }
    }
}
