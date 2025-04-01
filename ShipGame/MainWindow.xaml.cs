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

            // Spawn ships and assign fixed positions (1, 2, and 3)
            for (int i = 0; i < 3; i++)
            {
                SpawnNewShip(i, shipPaths[i]);
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
                //Debug.WriteLine($"Ships Count: {ships.Count}");


                if (ship.HasReachedEnd)
                {
                    shipsToRemove.Add(ship); // Mark for removal
                }
            }

            // Remove ships that reached the end and maintain order
            foreach (var ship in shipsToRemove)
            {
                int shipIndex = ships.IndexOf(ship); // Find index of removed ship
                ships.Remove(ship);
                GameCanvas.Children.Remove(ship.Shape);

                // Ensure we insert the new ship at the same index
                if (shipIndex >= 0)
                {
                    Ship newShip;
                    if (random.Next(2) == 0)
                    {
                        newShip = new CargoShip(3, Brushes.Yellow, 20, ship.Path);
                    }
                    else
                    {
                        newShip = new OilTanker(2, Brushes.Orange, 25, ship.Path);
                    }

                    ships.Insert(shipIndex, newShip); // Maintain order!
                    GameCanvas.Children.Add(newShip.Shape);
                }
            }

        }

        private void SpawnNewShip(int shipIndex, BezierPath path)
        {
            // Create a new ship with a fixed type and size for each path
            Ship newShip;
            if (random.Next(2) == 0)
            {
                newShip = new CargoShip(3, Brushes.Yellow, 20, path);
            }
            else
            {
                newShip = new OilTanker(2, Brushes.Orange, 25, path);
            }

            // Replace the old ship with the new one, preserving the order
            if (shipIndex < ships.Count)
            {
                ships[shipIndex] = newShip; // Replaces the old ship with the new one at the same index
            }
            else
            {
                ships.Add(newShip); // Add the new ship if it's the first spawn
            }

            // Ensure that the new ship is added to the canvas and that its movement starts correctly
            GameCanvas.Children.Add(newShip.Shape);
            newShip.MoveAlongPath();  // Ensure the ship starts moving immediately
        }


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
