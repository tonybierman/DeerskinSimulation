using DeerskinSimulation.Models;
using Xunit;

namespace DeerskinSimulation.Tests
{
    public class TradingEventStrategyTests
    {
        [Fact]
        public void ApplyEvent_ShouldIncreaseMoney_WhenEventOccurs()
        {
            // Arrange
            var participant = new MockParticipant("Trader1");
            var strategy = new TradingEventStrategy();
            participant.AddMoney(0); // Ensure money starts at 0

            // Act
            var result = strategy.ApplyEvent(participant);

            // Assert
            if (result.Contains("High demand increased the selling price"))
            {
                Assert.True(participant.Money > 0);
            }
        }

        [Fact]
        public void ApplyEvent_ShouldDecreaseSkins_WhenEventOccurs()
        {
            // Arrange
            var participant = new MockParticipant("Trader1");
            var strategy = new TradingEventStrategy();
            participant.AddSkins(100); // Ensure there are skins to remove

            // Act
            var result = strategy.ApplyEvent(participant);

            // Assert
            if (result.Contains("Damaged some skins"))
            {
                Assert.True(participant.Skins < 100);
            }
        }
    }
}
