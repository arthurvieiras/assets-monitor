using AssetsMonitor.Domain;
using AssetsMonitor.Domain.Messager;
using AssetsMonitor.Services;
using AssetsMonitor.Services.Messager;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;

namespace AssetsMonitor
{
    public class Program
    {
        private const int REFRESH_TIME = 10 * 1000;
        private const string SELL_MSG = "A ação {0} atingiu o valor de venda.\n Ação: {1} \nValor de venda definido: {2:0.00}\nValor atual: {3:0.00}";
        private const string BUY_MSG = "A ação {0} atingiu o valor de compra.\n Ação: {1} \nValor de compra definido: {2:0.00}\nValor atual: {3:0.00}";

        public static ServiceProvider Container { get; private set; }

        public static async System.Threading.Tasks.Task Main(string[] args)
        {
            setupContainer();

            EmailConfiguration emailConfig = null;
            try
            {
                using (var sr = new StreamReader("email.config.json"))
                {
                    emailConfig = JsonSerializer.Deserialize<EmailConfiguration>(sr.ReadToEnd());
                }
            }
            catch (IOException)
            {
                Console.WriteLine("Não foi possível ler o arquivo de configuração, abortando...");
                return;
            }
            catch (JsonException)
            {
                Console.WriteLine("Existem erros de preenchimento no arquivo de configuração, abortando...");
                return;
            }

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
                if(minValue > maxValue)
                {
                    Console.WriteLine("Erro ao ler parâmetros, o valor mínimo não deve ser maior que o valor máximo, certifique-se que os parâmetos se encontram no formato: ATIVO VALOR_MAXIMO VALOR_MINIMO");
                    return;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Erro ao ler parâmetros, certifique-se que se encontram no formato: ATIVO VALOR_MAXIMO VALOR_MINIMO");
                return;
            }

            Parameters p = new Parameters(assetName, minValue, maxValue);
            AssetService assetService = Container.GetService<AssetService>();
            IMessager messagerService = Container.GetService<IMessager>();
            try
            {
                messagerService.configure(emailConfig);
            } catch (ArgumentException)
            {
                Console.WriteLine("Algum dos parâmetros obrigatórios de configuração de SMTP não foram configurados, abortando...");
                return;
            }
            while(true)
            {
                Asset result = await assetService.getAssetInfoAsync(p.AssetName);
                if (result.Price > p.MaxValue)
                    await messagerService.sendAsync(string.Format(SELL_MSG, result.Name, result.Symbol, p.MaxValue, result.Price), "Sujestão de venda");
                if (result.Price < p.MinValue)
                    await messagerService.sendAsync(string.Format(BUY_MSG, result.Name, result.Symbol, p.MaxValue, result.Price), "Sujestão de compra");
                Thread.Sleep(REFRESH_TIME);
            }            
        }

        private static void setupContainer()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.TryAddTransient<HttpClient>();
            serviceCollection.TryAddSingleton<AssetService>();
            serviceCollection.AddTransient<IMessager, EmailMessager>();
            Container = serviceCollection.BuildServiceProvider();
        }
    }
}
