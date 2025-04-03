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
        private GameManager gameManager;

        public MainWindow()
        {
            InitializeComponent();
            gameManager = new GameManager(GameCanvas, LivesLabel, PointsLabel);
            gameManager.GameOver += OnGameOver;
            gameManager.RemoveShip += OnShipRemoved;

            try
            {
                gameManager.InitGame();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void OnGameOver()
        {
            MessageBox.Show("Game Over!");
        }

        // Event handler for RemoveShip event
        private void OnShipRemoved(Ship removedShip)
        {
            // Handle the ship removal logic, e.g., update UI, log the event, etc.
            Debug.WriteLine($"Ship removed: {removedShip.ShipType} | Current drift offset: {removedShip.driftOffset}");


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

            gameManager.FixShipPath(shipIndex);
        }
    }

}
