using AssetsMonitor.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AssetsMonitor.Services
{
    class AssetService
    {
        private HttpClient client;

        public AssetService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<Asset> getAssetInfoAsync(string assetName)
        {
            var stringTask = client.GetStringAsync("https://api.hgbrasil.com/finance/stock_price?key=bc75c5a6&symbol=" + assetName.ToLower());

            string msg = await stringTask;
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            return JsonSerializer.Deserialize<Response>(msg, serializeOptions).Results.GetValueOrDefault(assetName, null);
        }
        private class Response
        {
            public Dictionary<string, Asset> Results { get; set; }
        }
    }

}
