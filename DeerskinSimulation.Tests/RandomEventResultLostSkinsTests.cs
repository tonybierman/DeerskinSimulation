using Xunit;
using DeerskinSimulation.Models;

public class RandomEventResultLostSkinsTests
{
    [Fact]
    public void Constructor_ShouldInitializeCorrectly()
    {
        // Arrange
        int lostSkins = 5;

        // Act
        var result = new RandomEventResultLostSkins(lostSkins);

        // Assert
        Assert.Single(result.Records);
        Assert.Equal($"Lost {lostSkins} skins due to bad weather.", result.Records[0].Message);
        Assert.Equal("red", result.Records[0].Color);
        Assert.Equal("images/bad_fortune_256.jpg", result.Records[0].Image);
    }

    [Fact]
    public void ApplyActions_ShouldRemoveSkinsFromOriginator()
    {
        // Arrange
        int initialSkins = 20;
        int lostSkins = 5;
        int initialBag = 10;

        // Use a concrete implementation of ParticipantRole, such as RoleTrader
        var originator = new RoleTrader("Originator", 100.0, initialSkins) { CurrentBag = initialBag };

        var result = new RandomEventResultLostSkins(lostSkins);

        // Act
        result.ApplyActions(originator);

        // Assert
        Assert.Equal(initialSkins - lostSkins, originator.Skins);
        Assert.Equal(initialBag - lostSkins, originator.CurrentBag);
    }

    [Fact]
    public void ApplyActions_ShouldThrowException_WhenNotEnoughSkins()
    {
        // Arrange
        int initialSkins = 3;
        int lostSkins = 5;

        var originator = new RoleTrader("Originator", 100.0, initialSkins);

        var result = new RandomEventResultLostSkins(lostSkins);

        // Act & Assert
        Assert.Throws<ApplicationException>(() => result.ApplyActions(originator));
    }
}
