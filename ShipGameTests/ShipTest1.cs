using Microsoft.VisualStudio.CodeCoverage;
using ShipGame;
using System.Drawing;
using System.Windows;
using System.Windows.Media;


namespace ShipGameTests
{
    public class ShipTest1
    {
        [Fact]
        public void Ship_ShouldInitializeCorrectly()
        {
            // Arrange
            var ship = new CargoShip(3, Brushes.Yellow, 20, new BezierPath(new Point(100, 100), new Point(300, 200), new Point(500, 50), new Point(700, 100)));
            // Act & Assert
            Assert.Equal(3, ship.Speed);
        }
    }
}
