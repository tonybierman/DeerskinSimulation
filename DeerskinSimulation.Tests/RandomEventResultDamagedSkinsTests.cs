using Xunit;
using Moq;
using DeerskinSimulation.Models;

public class RandomEventResultDamagedSkinsTests
{
    [Fact]
    public void Constructor_ShouldInitializeCorrectly()
    {
        // Arrange
        int damagedSkins = 5;

        // Act
        var result = new RandomEventResultDamagedSkins(damagedSkins);

        // Assert
        Assert.Single(result.Records);
        Assert.Equal($"Damaged {damagedSkins} skins while forwarding.", result.Records[0].Message);
        Assert.Equal("red", result.Records[0].Color);
        Assert.Equal("images/packhorse_256.jpg", result.Records[0].Image);
    }

    [Fact]
    public void ApplyActions_ShouldRemoveDamagedSkinsFromOriginator()
    {
        // Arrange
        int initialSkins = 10;
        int damagedSkins = 5;

        var originatorMock = new Mock<ParticipantRole>("Originator", 0, initialSkins);
        originatorMock.Setup(o => o.RemoveSkins(damagedSkins)).Callback(() => originatorMock.Object.AddSkins(-damagedSkins));

        var result = new RandomEventResultDamagedSkins(damagedSkins);

        // Act
        result.ApplyActions(originatorMock.Object);

        // Assert
        originatorMock.Verify(o => o.RemoveSkins(damagedSkins), Times.Once);
    }
}
