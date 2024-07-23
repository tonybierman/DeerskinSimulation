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
            int skinsToExport = 100;
            exporter.AddSkins(skinsToExport);

            // Act
            var initialMoney = exporter.Money;
            var result = exporter.Export(skinsToExport);

            // Assert
            Assert.Equal(0, exporter.Skins);
            Assert.True(exporter.Money > initialMoney);
        }
    }
}
