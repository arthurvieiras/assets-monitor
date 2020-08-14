using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AssetsMonitor.Domain
{
    class Asset
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public double Price { get; set; }
    }
}
