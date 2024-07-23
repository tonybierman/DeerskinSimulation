using DeerskinSimulation.Models;
using Xunit;

namespace DeerskinSimulation.Tests
{
    public class HuntingEventStrategyTests
    {
        [Fact]
        public void ApplyEvent_ShouldIncreaseSkins_WhenEventOccurs()
        {
            // Arrange
            var participant = new MockParticipant("Hunter1");
            var strategy = new HuntingEventStrategy();
            participant.AddSkins(0); // Ensure skins start at 0

            // Act
            var result = strategy.ApplyEvent(participant);
            result.ApplyActions(participant);

            // Assert
            if (result.Records.Exists(record => record.Message.Contains("Found extra skins")))
            {
                Assert.True(participant.Skins > 0);
            }
        }

        [Fact]
        public void ApplyEvent_ShouldDecreaseSkins_WhenEventOccurs()
        {
            // Arrange
            var participant = new MockParticipant("Hunter1");
            var strategy = new HuntingEventStrategy();
            participant.AddSkins(100); // Ensure there are skins to remove

            // Act
            var result = strategy.ApplyEvent(participant);
            result.ApplyActions(participant);

            // Assert
            if (result.Records.Exists(record => record.Message.Contains("Lost some skins")))
            {
                Assert.True(participant.Skins < 100);
            }
        }
    }
}
