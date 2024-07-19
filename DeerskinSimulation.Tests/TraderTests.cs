using DeerskinSimulation.Models;
using Xunit;

namespace DeerskinSimulation.Tests
{
    public class TraderTests
    {
        [Fact]
        public void Trader_BuySkins_ShouldTransferSkinsAndMoney()
        {
            // Arrange
            var hunter = new Hunter("Hunter1");
            var trader = new Trader("Trader1");
            hunter.AddSkins(100);

            // Act
            var result = trader.BuySkins(hunter);

            // Assert
            Assert.Equal(0, hunter.Skins);
            Assert.Equal(100, trader.Skins);
            Assert.True(hunter.Money > 0);
            Assert.Contains("Sold 100 skins", result);
        }

        [Fact]
        public void Trader_TransportToExporter_ShouldTransferSkinsAndMoney()
        {
            // Arrange
            var trader = new Trader("Trader1");
            var exporter = new Exporter("Exporter1");
            trader.AddSkins(100);

            // Act
            var result = trader.TransportToExporter(exporter);

            // Assert
            Assert.Equal(0, trader.Skins);
            Assert.Equal(100, exporter.Skins);
            Assert.True(trader.Money > 0);
            Assert.Contains("Transported 100 skins", result);
        }
    }
}
