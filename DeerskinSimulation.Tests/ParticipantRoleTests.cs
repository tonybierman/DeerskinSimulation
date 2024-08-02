using System;
using Xunit;
using DeerskinSimulation.Models;
using Moq;

public class ParticipantRoleTests
{
    private class TestParticipantRole : ParticipantRole
    {
        public TestParticipantRole(string name, double initialMoney, int initialSkins)
            : base(name, initialMoney, initialSkins)
        {
        }

        public new void RaiseNotification(string message, string color)
        {
            base.RaiseNotification(message, color);
        }

        public void TestApplyRandomEvent(IRandomEventStrategy strategy)
        {
            ApplyRandomEvent(strategy);
        }
    }

    [Fact]
    public void AddSkins_ShouldIncreaseSkins()
    {
        // Arrange
        var participant = new TestParticipantRole("Test", 0, 0);

        // Act
        participant.AddSkins(5);

        // Assert
        Assert.Equal(5, participant.Skins);
    }

    [Fact]
    public void RemoveSkins_ShouldDecreaseSkins_WhenEnoughSkins()
    {
        // Arrange
        var participant = new TestParticipantRole("Test", 0, 10);

        // Act
        bool result = participant.RemoveSkins(5);

        // Assert
        Assert.True(result);
        Assert.Equal(5, participant.Skins);
    }

    [Fact]
    public void RemoveSkins_ShouldReturnFalse_WhenNotEnoughSkins()
    {
        // Arrange
        var participant = new TestParticipantRole("Test", 0, 5);

        // Act
        bool result = participant.RemoveSkins(10);

        // Assert
        Assert.False(result);
        Assert.Equal(5, participant.Skins);
    }

    [Fact]
    public void AddMoney_ShouldIncreaseMoney()
    {
        // Arrange
        var participant = new TestParticipantRole("Test", 0, 0);

        // Act
        participant.AddMoney(100.0);

        // Assert
        Assert.Equal(100.0, participant.Money);
    }

    [Fact]
    public void RemoveMoney_ShouldDecreaseMoney_WhenEnoughMoney()
    {
        // Arrange
        var participant = new TestParticipantRole("Test", 100.0, 0);

        // Act
        bool result = participant.RemoveMoney(50.0);

        // Assert
        Assert.True(result);
        Assert.Equal(50.0, participant.Money);
    }

    [Fact]
    public void RemoveMoney_ShouldReturnFalse_WhenNotEnoughMoney()
    {
        // Arrange
        var participant = new TestParticipantRole("Test", 50.0, 0);

        // Act
        bool result = participant.RemoveMoney(100.0);

        // Assert
        Assert.False(result);
        Assert.Equal(50.0, participant.Money);
    }

    [Fact]
    public void HasSkins_ShouldReturnTrue_WhenEnoughSkins()
    {
        // Arrange
        var participant = new TestParticipantRole("Test", 0, 10);

        // Act
        bool result = participant.HasSkins(5);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasSkins_ShouldReturnFalse_WhenNotEnoughSkins()
    {
        // Arrange
        var participant = new TestParticipantRole("Test", 0, 5);

        // Act
        bool result = participant.HasSkins(10);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasMoney_ShouldReturnTrue_WhenEnoughMoney()
    {
        // Arrange
        var participant = new TestParticipantRole("Test", 100.0, 0);

        // Act
        bool result = participant.HasMoney(50.0);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasMoney_ShouldReturnFalse_WhenNotEnoughMoney()
    {
        // Arrange
        var participant = new TestParticipantRole("Test", 50.0, 0);

        // Act
        bool result = participant.HasMoney(100.0);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ApplyRandomEvent_ShouldInvokeStrategy()
    {
        // Arrange
        var participant = new TestParticipantRole("Test", 100.0, 10);
        var mockStrategy = new Mock<IRandomEventStrategy>();
        mockStrategy.Setup(s => s.ApplyEvent(It.IsAny<ParticipantRole>()))
            .Returns(new EventResult(new EventRecord("Test event")));

        // Act
        participant.TestApplyRandomEvent(mockStrategy.Object);

        // Assert
        mockStrategy.Verify(s => s.ApplyEvent(participant), Times.Once);
    }
}
