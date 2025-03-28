using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ShipGame
{
    class CargoShip : Ship
    {
        public override string ShipType => "Cargo Ship";
        public CargoShip(double speed, Brush color, double size, BezierPath path)
            : base(speed, color, size, path) { }

        //public override double FuelConsumption { get; }
        //public override double GetFuelEfficiency()
        //{
        //    return Speed / FuelConsumption;
        //}
    }
}
