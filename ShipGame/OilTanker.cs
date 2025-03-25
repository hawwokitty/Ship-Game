using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ShipGame
{
    class OilTanker : Ship
    {
        public override string ShipType => "Oil Tanker";
        public OilTanker(int speed, Brush color, double size, double startX, double startY, double endX, double endY) : base(speed, color, size, startX, startY, endX, endY)
        {
        }

        public override double FuelConsumption { get; }
        public override double GetFuelEfficiency()
        {
            return Speed / FuelConsumption;
        }
    }
}
