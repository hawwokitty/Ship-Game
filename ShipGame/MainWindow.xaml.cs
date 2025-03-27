using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        //private List<BezierPath> shipPaths = new List<BezierPath>();
        public MainWindow()
        {
            InitializeComponent();
            InitGame();
        }

        private void InitGame()
        {
            // Create and add ships
            CargoShip ship1 = new CargoShip(2, Brushes.Yellow, 20, 100, 100, 700, 100);
            CargoShip ship2 = new CargoShip(2, Brushes.Yellow, 20, 100, 250, 700, 250);
            ships.Add(ship1);
            ships.Add(ship2);
            GameCanvas.Children.Add(ship1.Shape);
            GameCanvas.Children.Add(ship2.Shape);

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
                // Calculate the expected point on the correct Bezier curve path
                Point nextPoint = BezierCurve(t,
                    new Point(100 - offset, 100 - offset),
                    new Point(300 - offset, 200 - offset),
                    new Point(500 - offset, 50 - offset),
                    new Point(700 - offset, 100 - offset));

                // If the ship hasn't started drifting yet, roll the chance to start drifting
                if (!ship.HasStartedDrifting && random.Next(100) < 10) // % chance to start drifting
                {
                    ship.HasStartedDrifting = true; // Mark it as drifting
                }

                // If the ship has started drifting, it keeps drifting further off course
                if (ship.HasStartedDrifting)
                {
                    ship.DriftFurther(); // Make it drift up or down by 1 pixel
                    ship.MoveTo(nextPoint.X, ship.GetDriftedY());
                }
                else
                {
                    ship.MoveTo(nextPoint.X, nextPoint.Y);
                }

                // Update current position
                ship.CurrentX = nextPoint.X;
                ship.CurrentY = nextPoint.Y;
            }

            t += 0.0005; // Advance along the Bezier curve
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
