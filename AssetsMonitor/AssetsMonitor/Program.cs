using AssetsMonitor.Domain;
using System;

namespace AssetsMonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            string assetName = null;
            double minValue = 0;
            double maxValue = 0;
            try
            {
                assetName = args[0].ToUpper();
                minValue = Double.Parse(args[1]);
                maxValue = Double.Parse(args[2]);
            }
            catch (FormatException e)
            {
                Console.WriteLine("Erro ao ler parâmetros, certifique-se que se encontram no formato: ATIVO VALOR_MAXIMO VALOR_MINIMO");
                return;
            }

            Parameters p = new Parameters(assetName, minValue, maxValue);
        }
    }
}
