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
            int skinsToSell = 100;
            hunter.AddSkins(skinsToSell);

            // Act
            var result = trader.BuySkins(hunter, skinsToSell);

            // Assert
            Assert.Equal(0, hunter.Skins);
            Assert.Equal(skinsToSell, trader.Skins);
            Assert.True(hunter.Money > 0);
            Assert.Contains($"Sold {skinsToSell} skins", result);
        }

        [Fact]
        public void Trader_TransportToExporter_ShouldTransferSkinsAndMoney()
        {
            // Arrange
            var trader = new Trader("Trader1");
            var exporter = new Exporter("Exporter1");
            int skinsToTransport = 100;
            trader.AddSkins(skinsToTransport);

            // Act
            var result = trader.TransportToExporter(exporter);

            // Assert
            Assert.Equal(0, trader.Skins);
            Assert.Equal(skinsToTransport, exporter.Skins);
            Assert.True(trader.Money > 0);
            Assert.Contains($"Transported {skinsToTransport} skins", result);
        }
    }
}
