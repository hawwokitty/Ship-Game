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
        private List<BezierPath> shipPaths = new List<BezierPath>();
        public MainWindow()
        {
            InitializeComponent();
            InitGame();
        }

        private void InitGame()
        {
            // Define the three Bezier paths
            shipPaths.Add(new BezierPath(new Point(100, 100), new Point(300, 200), new Point(500, 50), new Point(700, 100)));
            shipPaths.Add(new BezierPath(new Point(100, 250), new Point(400, 200), new Point(500, 150), new Point(700, 250)));
            shipPaths.Add(new BezierPath(new Point(100, 400), new Point(400, 200), new Point(500, 450), new Point(700, 400)));

            // Spawn one ship per path
            foreach (var path in shipPaths)
            {
                SpawnNewShip(path);
            }

            // Timer for ship movement
            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromMilliseconds(16);
            gameTimer.Tick += GameLoop;
            gameTimer.Start();
        }
        private void GameLoop(object sender, EventArgs e)
        {
            List<Ship> shipsToRemove = new List<Ship>();

            foreach (var ship in ships)
            {
                ship.MoveAlongPath();

                if (ship.HasReachedEnd)
                {
                    shipsToRemove.Add(ship); // Mark for removal
                }
            }

            // Remove ships that reached the end
            foreach (var ship in shipsToRemove)
            {
                GameCanvas.Children.Remove(ship.Shape);
                ships.Remove(ship);

                // Find the path the ship was using and spawn a new one
                var path = ship.Path;
                SpawnNewShip(path);
            }
        }

        private void SpawnNewShip(BezierPath path)
        {
            Ship newShip;
            if (random.Next(2) == 0)
            {
                newShip = new CargoShip(3, Brushes.Yellow, 20, path);
            }
            else
            {
                newShip = new OilTanker(2, Brushes.Orange, 25, path);
            }

            ships.Add(newShip);
            GameCanvas.Children.Add(newShip.Shape);
        }

        //private Point BezierCurve(double t, Point p0, Point p1, Point p2, Point p3)
        //{
        //    double x = Math.Pow(1 - t, 3) * p0.X +
        //               3 * Math.Pow(1 - t, 2) * t * p1.X +
        //               3 * (1 - t) * Math.Pow(t, 2) * p2.X +
        //               Math.Pow(t, 3) * p3.X;

        //    double y = Math.Pow(1 - t, 3) * p0.Y +
        //               3 * Math.Pow(1 - t, 2) * t * p1.Y +
        //               3 * (1 - t) * Math.Pow(t, 2) * p2.Y +
        //               Math.Pow(t, 3) * p3.Y;

        //    return new Point(x, y);
        //}
        private void FixShipPath(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton == null) return;

            int shipIndex = -1;

            // Identify which ship the button corresponds to
            if (clickedButton.Content.ToString() == "Ship 1") shipIndex = 0;
            if (clickedButton.Content.ToString() == "Ship 2") shipIndex = 1;
            if (clickedButton.Content.ToString() == "Ship 3") shipIndex = 2;

            if (shipIndex >= 0 && shipIndex < ships.Count)
            {
                Ship ship = ships[shipIndex];

                // Reset drift effect
                ship.ResetDrift();
            }
        }

    }
}
