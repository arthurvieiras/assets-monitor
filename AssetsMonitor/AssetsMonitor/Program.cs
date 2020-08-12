using AssetsMonitor.Domain;
using AssetsMonitor.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.ComponentModel;
using System.Net.Http;

namespace AssetsMonitor
{
    public class Program
    {
        public static ServiceProvider Container { get; private set; }

        public static async System.Threading.Tasks.Task Main(string[] args)
        {
            setupContainer();

            if (args.Length < 3)
            {
                Console.WriteLine("Número de parâmetros inferior ao especificado, por favor informe parâmetros no formato: ATIVO VALOR_MAXIMO VALOR_MINIMO");
                return;
            }

            string assetName = null;
            double minValue = 0;
            double maxValue = 0;

            try
            {
                assetName = args[0].ToUpper();
                minValue = Double.Parse(args[1]);
                maxValue = Double.Parse(args[2]);
            }
            catch (FormatException)
            {
                Console.WriteLine("Erro ao ler parâmetros, certifique-se que se encontram no formato: ATIVO VALOR_MAXIMO VALOR_MINIMO");
                return;
            }

            Parameters p = new Parameters(assetName, minValue, maxValue);
            AssetService service = Container.GetService<AssetService>();
            string result = await service.getAssetInfoAsync(p.AssetName);
            return;
        }

        private static void setupContainer()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.TryAddTransient<HttpClient>();
            serviceCollection.TryAddSingleton<AssetService>();
            Container = serviceCollection.BuildServiceProvider();
        }
    }
}
