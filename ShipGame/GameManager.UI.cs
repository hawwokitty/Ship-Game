using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipGame
{
    partial class GameManager
    {
        public void UpdateUI()
        {
            livesLabel.Content = $"Lives: {lives}";
            pointsLabel.Content = $"Points: {points}";
        }

        private void DrawShip(Ship ship)
        {
            gameCanvas.Children.Add(ship.Shape);
        }

        private void EraseShip(Ship ship)
        {
            gameCanvas.Children.Remove(ship.Shape);
        }
    }
}
