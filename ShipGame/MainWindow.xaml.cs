using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ShipGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Ship> ships = new List<Ship>();
        private Random random = new Random();
        private DispatcherTimer gameTimer;
        private double t = 0;
        private int offset = 10;
        public MainWindow()
        {
            InitializeComponent();
            InitGame();
        }

        private void InitGame()
        {
            // Create and add ships
            CargoShip ship1 = new CargoShip(2, Brushes.Yellow, 20, 100, 100, 700, 100);
            ships.Add(ship1);
            GameCanvas.Children.Add(ship1.Shape);

            // Timer for ship movement
            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromMilliseconds(16);
            gameTimer.Tick += GameLoop;
            gameTimer.Start();
        }
        private void GameLoop(object sender, EventArgs e)
        {
            if (t > 1) return;

            foreach (var ship in ships)
            {
                // Expected point on the correct path
                Point nextPoint = BezierCurve(t, new Point(100-offset, 100 - offset), new Point(300 - offset, 200 - offset), new Point(500 - offset, 50 - offset), new Point(700 - offset, 100 - offset));

                if (random.Next(100) > 90)
                {
                    ship.DriftFurther();
                }

                // If the ship is off course, make it drift more
                if (ship.IsOffCourse(20, nextPoint.Y)) // 20-pixel margin for deviation
                {
                    ship.DriftFurther(); // Make the ship drift even more
                    ship.MoveTo(nextPoint.X, ship.GetDriftedY()); // Move it even further off course
                }
                else
                {
                    ship.MoveTo(nextPoint.X, nextPoint.Y);
                }
            }

            t += 0.005;
        }


        private Point BezierCurve(double t, Point p0, Point p1, Point p2, Point p3)
        {
            double x = Math.Pow(1 - t, 3) * p0.X +
                       3 * Math.Pow(1 - t, 2) * t * p1.X +
                       3 * (1 - t) * Math.Pow(t, 2) * p2.X +
                       Math.Pow(t, 3) * p3.X;

            double y = Math.Pow(1 - t, 3) * p0.Y +
                       3 * Math.Pow(1 - t, 2) * t * p1.Y +
                       3 * (1 - t) * Math.Pow(t, 2) * p2.Y +
                       Math.Pow(t, 3) * p3.Y;

            return new Point(x, y);
        }
    }
}
