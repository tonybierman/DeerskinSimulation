using DeerskinSimulation.Models;
using Xunit;

namespace DeerskinSimulation.Tests
{
    public class RandomEventLostSkinsTests
    {
        [Fact]
        public void RandomEventLostSkins_ShouldInitializeCorrectly()
        {
            // Arrange
            int lostSkins = 10;

            // Act
            var randomEvent = new RandomEventResultLostSkins(lostSkins);

            // Assert
            Assert.Single(randomEvent.Records);
            Assert.Equal("Lost 10 skins due to bad weather.", randomEvent.Records[0].Message);
            Assert.Equal("red", randomEvent.Records[0].Color);
            Assert.Equal("images/bad_fortune_256.jpg", randomEvent.Records[0].Image);
            Assert.NotNull(randomEvent.OriginatorAction);
            Assert.Null(randomEvent.RecipientAction);
        }

        [Fact]
        public void ApplyActions_ShouldRemoveSkinsFromParticipant()
        {
            // Arrange
            int lostSkins = 10;
            var participant = new MockParticipant("Participant1");
            participant.AddSkins(20);
            var randomEvent = new RandomEventResultLostSkins(lostSkins);

            // Act
            randomEvent.ApplyActions(participant);

            // Assert
            Assert.Equal(10, participant.Skins);
        }
    }
}
