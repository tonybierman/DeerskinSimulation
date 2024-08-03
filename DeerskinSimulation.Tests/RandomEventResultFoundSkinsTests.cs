using Xunit;
using Moq;
using DeerskinSimulation.Models;

public class RandomEventResultFoundSkinsTests
{
    [Fact]
    public void Constructor_ShouldInitializeCorrectly()
    {
        // Arrange
        int extraSkins = 10;

        // Act
        var result = new RandomEventResultFoundSkins(extraSkins);

        // Assert
        Assert.Single(result.Records);
        Assert.Equal($"Found {extraSkins} extra skins during the hunt!", result.Records[0].Message);
        Assert.Equal("green", result.Records[0].Color);
        Assert.Equal("images/good_fortune_256.jpg", result.Records[0].Image);
    }

    [Fact]
    public void ApplyActions_ShouldAdjustSkinsAndCurrentBag()
    {
        // Arrange
        int initialSkins = 15;
        int extraSkins = 10;

        var participantMock = new Mock<ParticipantRole>("Participant", 0, initialSkins);
        participantMock.Object.CurrentBag = initialSkins;

        participantMock.Setup(p => p.RemoveSkins(extraSkins))
            .Callback(() => participantMock.Object.AddSkins(-extraSkins));

        var result = new RandomEventResultFoundSkins(extraSkins);

        // Act
        result.ApplyActions(participantMock.Object);

        // Assert
        participantMock.Verify(p => p.RemoveSkins(extraSkins), Times.Once);
        Assert.Equal(initialSkins - extraSkins, participantMock.Object.CurrentBag);
    }
}
