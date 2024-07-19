using DeerskinSimulation.Models;
using Xunit;

namespace DeerskinSimulation.Tests
{
    public class ParticipantTests
    {
        [Fact]
        public void Participant_AddSkins_ShouldIncreaseSkins()
        {
            // Arrange
            var participant = new MockParticipant("Participant1");

            // Act
            participant.AddSkins(100);

            // Assert
            Assert.Equal(100, participant.Skins);
        }

        [Fact]
        public void Participant_RemoveSkins_ShouldDecreaseSkins()
        {
            // Arrange
            var participant = new MockParticipant("Participant1");
            participant.AddSkins(100);

            // Act
            participant.RemoveSkins(50);

            // Assert
            Assert.Equal(50, participant.Skins);
        }

        [Fact]
        public void Participant_AddMoney_ShouldIncreaseMoney()
        {
            // Arrange
            var participant = new MockParticipant("Participant1");

            // Act
            participant.AddMoney(100);

            // Assert
            Assert.Equal(100, participant.Money);
        }

        [Fact]
        public void Participant_RemoveMoney_ShouldDecreaseMoney()
        {
            // Arrange
            var participant = new MockParticipant("Participant1");
            participant.AddMoney(100);

            // Act
            participant.RemoveMoney(50);

            // Assert
            Assert.Equal(50, participant.Money);
        }
    }

    public class MockParticipant : Participant
    {
        public MockParticipant(string name) : base(name, 0) { }

        public override string Hunt() => string.Empty;
    }
}
