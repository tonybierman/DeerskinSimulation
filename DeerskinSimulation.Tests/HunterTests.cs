﻿using DeerskinSimulation.Models;
using Xunit;

namespace DeerskinSimulation.Tests
{
    public class HunterTests
    {
        [Fact]
        public void Hunter_Hunt_ShouldIncreaseSkinsAndDecreaseMoney()
        {
            // Arrange
            var hunter = new RoleHunter("Hunter1");
            int packhorses = 3; // Assuming the hunter uses 3 packhorses for the hunt

            // Act
            var initialMoney = hunter.Money;
            hunter.Hunt(packhorses);

            // Assert
            Assert.True(hunter.Skins > 0);
            Assert.True(hunter.Money < initialMoney);
        }

        [Fact]
        public void Hunter_SellToTrader_ShouldTransferSkinsAndMoney()
        {
            // Arrange
            var hunter = new RoleHunter("Hunter1");
            var trader = new RoleTrader("Trader1");
            int skinsToSell = 100;
            hunter.AddSkins(skinsToSell);

            // Act
            var result = hunter.ForwardToTrader(trader, skinsToSell);

            // Assert
            Assert.Equal(0, hunter.Skins);
            Assert.Equal(skinsToSell, trader.Skins);
            Assert.True(hunter.Money > 0);
        }
    }
}
