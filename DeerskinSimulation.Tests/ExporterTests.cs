using DeerskinSimulation.Models;
using Xunit;

namespace DeerskinSimulation.Tests
{
    public class ExporterTests
    {
        [Fact]
        public void Exporter_Export_ShouldDecreaseSkinsAndIncreaseMoney()
        {
            // Arrange
            var exporter = new Exporter("Exporter1");
            exporter.AddSkins(100);

            // Act
            var initialMoney = exporter.Money;
            var result = exporter.Export();

            // Assert
            Assert.Equal(0, exporter.Skins);
            Assert.True(exporter.Money > initialMoney);
            Assert.Contains("Exported 100 skins", result);
        }
    }
}
