using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ShipGame
{
    public abstract class Ship
    {
        public virtual string ShipType { get; set; }
        public double Speed { get; set; }
        public double StartX, StartY, EndX, EndY;
        public double CurrentX, CurrentY;
        public double DriftAmount = 0;
        public bool HasStartedDrifting { get; set; } = false; 
        public abstract double FuelConsumption { get; }
        public Random Random = new Random();

        public Ellipse Shape { get; private set; }

        public Ship(double speed, Brush color, double size, double startX, double startY, double endX, double endY)
        {
            Speed = speed;
            StartX = startX;
            StartY = startY;
            EndX = endX;
            EndY = endY;

            CurrentX = startX;
            CurrentY = startY;

            Shape = new Ellipse
            {
                Width = size,
                Height = size,
                Fill = color
            };
        }

        public void MoveTo(double x, double y)
        {
            Canvas.SetLeft(Shape, x);
            Canvas.SetTop(Shape, y);
            CurrentX = x;
            CurrentY = y;
        }

        public abstract double GetFuelEfficiency();
        //public bool IsOffCourse(double allowedDeviation, double correctY)
        //{
        //    return Math.Abs(CurrentY - correctY) > allowedDeviation;
        //}

        public void DriftFurther()
        {
            DriftAmount += (Random.Next(2) == 0) ? 0.5 : -0.5;
        }

        public double GetDriftedY()
        {
            return CurrentY + DriftAmount;
        }
        public void RestartCourse()
        {
            // Reset back on course
            CurrentX = StartX;
            CurrentY = StartY;
        }

    }
}
