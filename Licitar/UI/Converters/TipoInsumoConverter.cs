using System;
using System.Globalization;
using System.Windows.Data;

namespace Licitar
{
    class TipoInsumoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((tipoInsumo)value)
            {
                case tipoInsumo.Composicao:
                    return "Composição";
                case tipoInsumo.MaoDeObra:
                    return "Mão de Obra";
                case tipoInsumo.Material:
                    return "Material";
                case tipoInsumo.Outros:
                    return "Outros";
                case tipoInsumo.Titulo:
                    return "Titulo";
            }
            return "Material";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case "Composição":
                    return tipoInsumo.Composicao;
                case "Mão de Obra":
                    return tipoInsumo.MaoDeObra;
                case "Material":
                    return tipoInsumo.Material;
                case "Outros":
                    return tipoInsumo.Outros;
                case "Titulo":
                    return tipoInsumo.Titulo;
            }
            return tipoInsumo.Material;
        }
    }
}
