using Xunit;
using Moq;
using DeerskinSimulation.Models;

public class RandomEventResultHighDemandTests
{
    [Fact]
    public void Constructor_ShouldInitializeCorrectly()
    {
        // Arrange
        double bonusMoney = 50.0;

        // Act
        var result = new RandomEventResultHighDemand(bonusMoney);

        // Assert
        Assert.Single(result.Records);
        Assert.Equal($"High demand increased the selling price by {bonusMoney}.", result.Records[0].Message);
        Assert.Equal("green", result.Records[0].Color);
        Assert.Equal("images/packhorse_256.jpg", result.Records[0].Image);
    }

    [Fact]
    public void ApplyActions_ShouldAddBonusMoneyToOriginator()
    {
        // Arrange
        double initialMoney = 100.0;
        double bonusMoney = 50.0;

        // Use a concrete implementation of ParticipantRole, such as RoleTrader
        var originator = new RoleTrader("Originator", initialMoney, 0);

        var result = new RandomEventResultHighDemand(bonusMoney);

        // Act
        result.ApplyActions(originator);

        // Assert
        Assert.Equal(initialMoney + bonusMoney, originator.Money);
    }

    [Fact]
    public void ApplyActions_ShouldRemoveBonusMoneyFromRecipient()
    {
        // Arrange
        double initialMoney = 150.0;
        double bonusMoney = 50.0;

        // Use a concrete implementation of ParticipantRole, such as RoleTrader
        var recipient = new RoleTrader("Recipient", initialMoney, 0);

        var result = new RandomEventResultHighDemand(bonusMoney);

        // Act
        result.ApplyActions(new RoleTrader("Originator", 0, 0), recipient);

        // Assert
        Assert.Equal(initialMoney - bonusMoney, recipient.Money);
    }

    [Fact]
    public void ApplyActions_ShouldNotFailWhenRecipientIsNull()
    {
        // Arrange
        double bonusMoney = 50.0;

        var originator = new RoleTrader("Originator", 100.0, 0);
        var result = new RandomEventResultHighDemand(bonusMoney);

        // Act
        result.ApplyActions(originator, null);

        // Assert
        Assert.Equal(150.0, originator.Money);
    }
}
