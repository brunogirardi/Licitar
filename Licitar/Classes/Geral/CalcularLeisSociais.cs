namespace Licitar
{
    public class CalcularLeisSociais
    {

        public static double Calcular (double valor, tipoInsumo tipo, string unidade)
        {
            if (tipo == tipoInsumo.MaoDeObra)
            {
                if ((unidade == "MÊS") && (unidade == "MES"))
                {
                    return valor * .84d;
                } else
                {
                    return valor * 1.28D;
                }
            }
            return valor;
        }

    }
}
