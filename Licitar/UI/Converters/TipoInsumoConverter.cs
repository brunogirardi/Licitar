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
                case tipoInsumo.Equipamentos:
                    return "Equipamentos";
                case tipoInsumo.Verbas:
                    return "Verbas";
                case tipoInsumo.Titulo:
                    return "Titulo";
                case tipoInsumo.Indefinido:
                    return "Indefinido";
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
                case "Equipamentos":
                    return tipoInsumo.Equipamentos;
                case "Verbas":
                    return tipoInsumo.Verbas;
                case "Titulo":
                    return tipoInsumo.Titulo;
                case "Indefinido":
                    return tipoInsumo.Indefinido;
            }
            return tipoInsumo.Material;
        }
    }
}
