using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ShipGame
{
    public class BezierPath
    {
        public Point P0, P1, P2, P3;

        public BezierPath(Point p0, Point p1, Point p2, Point p3)
        {
            P0 = p0;
            P1 = p1;
            P2 = p2;
            P3 = p3;
        }

        public Point GetPoint(double t)
        {
            double x = Math.Pow(1 - t, 3) * P0.X +
                       3 * Math.Pow(1 - t, 2) * t * P1.X +
                       3 * (1 - t) * Math.Pow(t, 2) * P2.X +
                       Math.Pow(t, 3) * P3.X;

            double y = Math.Pow(1 - t, 3) * P0.Y +
                       3 * Math.Pow(1 - t, 2) * t * P1.Y +
                       3 * (1 - t) * Math.Pow(t, 2) * P2.Y +
                       Math.Pow(t, 3) * P3.Y;

            return new Point(x, y);
        }
    }
}
