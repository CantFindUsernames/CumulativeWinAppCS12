using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CulmativeWinAppCS12
{
    class Plant
    {
        double _moistureMin;
        double _moistureMax;
        string _moisture;
        public Plant(string name, string moisture)
        {
            this.Name = name;
            this.Moisture = moisture;
        }
        public Plant() {
            this.Name = null;
            this.Moisture = null;
        }
        public string Name { get; set; }
        public string Moisture
        {
            get { return _moisture; }
            set
            {
                _moisture = value;
                if (this.Moisture == "Low") { _moistureMin = 0.2; _moistureMax = 0.4; }
                if (this.Moisture == "Medium") { _moistureMin = 0.4; _moistureMax = 0.6; }
                if (this.Moisture == "High") { _moistureMin = 0.6; _moistureMax = 0.8; }
                //else {_moistureMin = 0.5; _moistureMax = 0.5; }

            }
        }
        public double MoistureMin
        {
            get { return _moistureMin; }
            set { value = _moistureMin; }
            
        }
        public double MoistureMax
        {
            get { return _moistureMax; }
            set {value = _moistureMax; } 
        }
    }
}
