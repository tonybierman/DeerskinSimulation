using DeerskinSimulation.Models;
using Xunit;

namespace DeerskinSimulation.Tests
{
    public class TraderTests
    {
        [Fact]
        public void Trader_TransportToExporter_ShouldTransferSkinsAndMoney()
        {
            // Arrange
            var trader = new Trader("Trader1");
            var exporter = new Exporter("Exporter1");
            int skinsToTransport = 100;
            trader.AddSkins(skinsToTransport);

            // Act
            var result = trader.TransportToExporter(exporter, skinsToTransport);

            // Assert
            Assert.Equal(0, trader.Skins);
            Assert.Equal(skinsToTransport, exporter.Skins);
            Assert.True(trader.Money > 0);
        }
    }
}
