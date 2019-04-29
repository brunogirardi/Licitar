using System;
using System.Globalization;
using System.Windows.Data;

namespace Licitar
{
    class BdiToIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int i = 0;
            foreach (Bdi item in Factory.Bdis)
            {
                if (item.Equals((Bdi)value)) {
                    return i;
                }
                i++;
            }

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Factory.Bdis[(int)value];
        }
    }
}
