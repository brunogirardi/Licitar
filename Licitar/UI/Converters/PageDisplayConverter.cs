using System;
using System.Globalization;
using System.Windows.Data;

namespace Licitar
{
    class PageDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((PageSystem)value)
            {
                case PageSystem.Orcamento:
                    return new OrcamentoPage(Factory.AccessoAppProvider);
                case PageSystem.BaseDados:
                    return new BaseInsumosPage();
            }

            return new OrcamentoPage(Factory.AccessoAppProvider);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
