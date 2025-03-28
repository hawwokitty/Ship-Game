using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ShipGame
{
    public abstract class Ship
    {
        public virtual string ShipType { get; set; }
        public Ellipse Shape { get; private set; }
        public BezierPath Path { get; private set; }
        private double t = 0;
        public double Speed;
        private bool drifting = false;
        private int driftOffset = 0;
        private Random shipRandom;

        public bool HasReachedEnd => t > 1;

        public Ship(double speed, Brush color, double size, BezierPath path)
        {
            Speed = speed;
            Path = path;

            shipRandom = new Random(Guid.NewGuid().GetHashCode());

            Shape = new Ellipse
            {
                Width = size,
                Height = size,
                Fill = color
            };

            MoveTo(Path.GetPoint(0));
        }

        public void MoveAlongPath()
        {
            if (t > 1) return;

            // Start drifting randomly (each ship decides independently)
            if (!drifting && shipRandom.Next(100) < 10)
            {
                drifting = true;
            }

            if (drifting)
            {
                driftOffset += shipRandom.Next(2) == 0 ? -1 : 1; // Drift up or down randomly
            }

            Point newPosition = Path.GetPoint(t);
            MoveTo(new Point(newPosition.X, newPosition.Y + driftOffset));

            t += Speed / 1000.0;
        }

        public void MoveTo(Point position)
        {
            Canvas.SetLeft(Shape, position.X);
            Canvas.SetTop(Shape, position.Y);
        }

        public void ResetDrift()
        {
            drifting = false;
            driftOffset = 0;
        }

    }
}
