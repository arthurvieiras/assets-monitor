using AssetsMonitor.Domain;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
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

        public async Task<string> getAssetInfoAsync(string assetName)
        {
            var stringTask = client.GetStringAsync("https://api.hgbrasil.com/finance/stock_price?key=bc75c5a6&symbol=" + assetName.ToLower());

            var msg = await stringTask;
            return msg;
        }
    }
}
