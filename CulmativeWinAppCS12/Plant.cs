using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CulmativeWinAppCS12
{
    class Plant
    {
        public Plant(string name, double moisture)
        {
            this.Name = name;
            this.Moisture = moisture;
        }
        public string Name { get; set; }
        public double Moisture { get; set; }
    }
}
