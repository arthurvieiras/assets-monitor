using System;
using System.Collections.Generic;
using System.Text;

namespace AssetsMonitor.Domain
{
    class Parameters
    {
        
        private string assetName;
        private double minValue;
        private double maxValue;

        public string AssetName => assetName;
        public double MinValue => minValue;
        public double MaxValue => maxValue;

        public Parameters(string assetName, double minValue, double maxValue)
        {
            this.assetName = assetName;
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

    }
}
