using DeerskinSimulation.Models;
using Xunit;

namespace DeerskinSimulation.Tests
{
    public class HunterTests
    {
        [Fact]
        public void Hunter_Hunt_ShouldIncreaseSkinsAndDecreaseMoney()
        {
            // Arrange
            var hunter = new Hunter("Hunter1");

            // Act
            var initialMoney = hunter.Money;
            hunter.Hunt();

            // Assert
            Assert.True(hunter.Skins > 0);
            Assert.True(hunter.Money < initialMoney);
        }

        [Fact]
        public void Hunter_SellToTrader_ShouldTransferSkinsAndMoney()
        {
            // Arrange
            var hunter = new Hunter("Hunter1");
            var trader = new Trader("Trader1");
            hunter.AddSkins(100);

            // Act
            var result = hunter.SellToTrader(trader);

            // Assert
            Assert.Equal(0, hunter.Skins);
            Assert.Equal(100, trader.Skins);
            Assert.True(hunter.Money > 0);
            Assert.Contains("Sold 100 skins", result);
        }
    }
}
