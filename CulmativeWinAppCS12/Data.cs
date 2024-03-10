using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CulmativeWinAppCS12
{
    class Data
    {
        public Data(double temperature, double humidity, double moisture, double light)
        {
            this.Temperature = temperature;
            this.Humidity = humidity;
            this.Moisture = moisture;
            this.Light = light;

        }

        public double Temperature { get; set; } 
        public double Humidity { get; set; }
        public double Moisture { get; set; }
        public double Light { get; set; }
    }
}
