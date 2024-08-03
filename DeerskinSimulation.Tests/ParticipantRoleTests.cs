using System;
using Xunit;
using DeerskinSimulation.Models;

public class ParticipantRoleTests
{
    private class TestParticipantRole : ParticipantRole
    {
        public TestParticipantRole(string name, double initialMoney, int initialSkins)
            : base(name, initialMoney, initialSkins)
        {
        }
    }

    [Fact]
    public void AddSkins_ShouldIncreaseSkinCount()
    {
        // Arrange
        var participant = new TestParticipantRole("Tester", 0, 0);

        // Act
        participant.AddSkins(10);

        // Assert
        Assert.Equal(10, participant.Skins);
    }

    [Fact]
    public void RemoveSkins_ShouldDecreaseSkinCount_WhenEnoughSkinsExist()
    {
        // Arrange
        var participant = new TestParticipantRole("Tester", 0, 10);

        // Act
        participant.RemoveSkins(5);

        // Assert
        Assert.Equal(5, participant.Skins);
    }

    [Fact]
    public void RemoveSkins_ShouldThrowException_WhenNotEnoughSkinsExist()
    {
        // Arrange
        var participant = new TestParticipantRole("Tester", 0, 5);

        // Act & Assert
        Assert.Throws<ApplicationException>(() => participant.RemoveSkins(10));
    }

    [Fact]
    public void AddMoney_ShouldIncreaseMoneyAmount()
    {
        // Arrange
        var participant = new TestParticipantRole("Tester", 0, 0);

        // Act
        participant.AddMoney(100.0);

        // Assert
        Assert.Equal(100.0, participant.Money);
    }

    [Fact]
    public void RemoveMoney_ShouldDecreaseMoneyAmount_WhenEnoughMoneyExists()
    {
        // Arrange
        var participant = new TestParticipantRole("Tester", 100.0, 0);

        // Act
        participant.RemoveMoney(50.0);

        // Assert
        Assert.Equal(50.0, participant.Money);
    }

    [Fact]
    public void RemoveMoney_ShouldThrowException_WhenNotEnoughMoneyExists()
    {
        // Arrange
        var participant = new TestParticipantRole("Tester", 50.0, 0);

        // Act & Assert
        Assert.Throws<ApplicationException>(() => participant.RemoveMoney(100.0));
    }

    [Fact]
    public void HasSkins_ShouldReturnTrue_WhenEnoughSkinsExist()
    {
        // Arrange
        var participant = new TestParticipantRole("Tester", 0, 10);

        // Act
        var result = participant.HasSkins(5);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasSkins_ShouldReturnFalse_WhenNotEnoughSkinsExist()
    {
        // Arrange
        var participant = new TestParticipantRole("Tester", 0, 5);

        // Act
        var result = participant.HasSkins(10);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasMoney_ShouldReturnTrue_WhenEnoughMoneyExists()
    {
        // Arrange
        var participant = new TestParticipantRole("Tester", 100.0, 0);

        // Act
        var result = participant.HasMoney(50.0);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasMoney_ShouldReturnFalse_WhenNotEnoughMoneyExists()
    {
        // Arrange
        var participant = new TestParticipantRole("Tester", 50.0, 0);

        // Act
        var result = participant.HasMoney(100.0);

        // Assert
        Assert.False(result);
    }
}
