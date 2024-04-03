using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CulmativeWinAppCS12
{
    class Plant
    {
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
        public string Moisture { get; set; }
        public double MoistureMin
        {
            get { return this.MoistureMin; }
            set { if (this.Moisture == "Low") { this.MoistureMin = 0.2; }
                if (this.Moisture == "Medium") { this.MoistureMin = 0.4; }
                if (this.Moisture == "High") { this.MoistureMin = 0.6; }
                else { this.MoistureMin = 0.5; }
            }
        }
        public double MoistureMax
        {
            get { return this.MoistureMax; }
            set
            {
                if (this.Moisture == "Low") { this.MoistureMax = 0.4; }
                if (this.Moisture == "Medium") { this.MoistureMax = 0.6; }
                if (this.Moisture == "High") { this.MoistureMax = 0.8; }
                else { this.MoistureMax = 0.5;}
            } 
        }
    }
}
