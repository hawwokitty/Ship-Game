using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ShipGame
{
    public abstract class Ship
    {
        public virtual string ShipType { get; set; }
        public Ellipse Shape { get; private set; }
        public BezierPath Path { get; private set; }
        public double t = 0;
        public double Speed;
        private bool drifting = false;
        private int driftOffset = 0;
        private Random shipRandom;
        private bool fixedPath = false;

        public bool HasReachedEnd => t >= 1.0;


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

            // Only allow drifting after ResetDrift, with a 5% chance if path is fixed
            if (fixedPath && !drifting && shipRandom.Next(1000) < 1)
            {
                drifting = true;
            }

            // If not fixed path, allow 10% chance to start drifting
            if (!fixedPath && !drifting && shipRandom.Next(100) < 10)
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
            Canvas.SetTop(Shape, position.Y - (Shape.Height / 2)); // Centering the ship

            Application.Current.Dispatcher.Invoke(() => { }, DispatcherPriority.Render);

        }


        public void ResetDrift()
        {
            drifting = false;
            driftOffset = 0;
            fixedPath = true;

            //// Reset movement progress to restart the ship
            //t = 0; // Reset path progress to start over
        }


    }
}
