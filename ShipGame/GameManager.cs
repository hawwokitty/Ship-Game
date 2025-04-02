using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Controls;

namespace ShipGame
{
    class GameManager
    {
        private List<Ship> ships = new List<Ship>();
        private List<BezierPath> shipPaths = new List<BezierPath>();
        private Random random = new Random();
        private DispatcherTimer gameTimer;

        private int lives = 3;
        private int points = 0;
        private double speedMultiplier = 1.0;
        private Canvas gameCanvas;
        private Label livesLabel, pointsLabel;

        // Define the event delegate for gameover
        public delegate void GameEventHandler();
        // Define the event that will be triggered when the game is over
        public event GameEventHandler GameOver;

        // Define a new delegate for the RemoveShip event
        public delegate void RemoveShipEventHandler(Ship removedShip);
        // Define the event for when a ship is removed
        public event RemoveShipEventHandler RemoveShip;

        public GameManager(Canvas gameCanvas, Label livesLabel, Label pointsLabel)
        {
            this.gameCanvas = gameCanvas;
            this.livesLabel = livesLabel;
            this.pointsLabel = pointsLabel;
        }

        public void InitGame()
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
            speedMultiplier += 0.000001; // Gradually increase speed each frame

            if (lives <= 0)
            {
                gameTimer.Stop();
                // Trigger the GameOver event
                GameOver?.Invoke();
                return;
            }

            List<Ship> shipsToRemove = new List<Ship>();

            foreach (var ship in ships)
            {
                ship.Speed *= speedMultiplier;
                ship.MoveAlongPath();

                // Remove the ship if it has reached the end
                if (ship.HasReachedEnd)
                {
                    shipsToRemove.Add(ship);
                    points++;
                }
                // Remove the ship if it's too far from the path (drift check)
                else if (Math.Abs(ship.driftOffset) > 20 && ship.t > 1)
                {
                    shipsToRemove.Add(ship);
                    lives--; // Failed path = Lose a life
                }
            }

            // Remove ships that reached the end and maintain order
            foreach (var ship in shipsToRemove)
            {
                int shipIndex = ships.IndexOf(ship); // Find index of removed ship
                ships.Remove(ship);
                //Debug.WriteLine($"Removing Ship: {ship.ShipType} | Current offset: {ship.driftOffset}");
                gameCanvas.Children.Remove(ship.Shape);

                // Trigger RemoveShip event for the removed ship
                RemoveShip?.Invoke(ship);

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
                    gameCanvas.Children.Add(newShip.Shape);
                }
            }
            // 🔹 Update the Label UI with new points
            livesLabel.Content = $"Lives: {lives}";
            pointsLabel.Content = $"Points: {points}";
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
            gameCanvas.Children.Add(newShip.Shape);
            newShip.MoveAlongPath();  // Ensure the ship starts moving immediately
        }

        public void FixShipPath(int shipIndex)
        {
            if (shipIndex >= 0 && shipIndex < ships.Count)
            {
                ships[shipIndex].ResetDrift();
            }
        }

    }
}
