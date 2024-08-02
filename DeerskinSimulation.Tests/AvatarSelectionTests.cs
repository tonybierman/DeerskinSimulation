using Bunit;
using Xunit;
using Moq;
using DeerskinSimulation.Pages;
using DeerskinSimulation.Models;
using Microsoft.Extensions.DependencyInjection;

public class AvatarSelectionTests : TestContext
{
    [Fact]
    public void AvatarsAreDisplayedCorrectly()
    {
        // Arrange
        var mockSession = new Mock<IStateContainer>();
        Services.AddSingleton(mockSession.Object);

        // Act
        var component = RenderComponent<AvatarSelection>();
        var avatarImages = component.FindAll("img.avatar");

        // Assert
        Assert.Equal(6, avatarImages.Count); // Check if all avatars are displayed
    }

    [Fact]
    public void ClickingAvatarUpdatesSelectedAvatar()
    {
        // Arrange
        var mockSession = new Mock<IStateContainer>();
        Services.AddSingleton(mockSession.Object);
        var component = RenderComponent<AvatarSelection>();
        var firstAvatar = component.Find("img.avatar");

        // Act
        firstAvatar.Click();
        var selectedAvatar = component.Find("img.avatar-selected");

        // Assert
        Assert.NotNull(selectedAvatar); // Check if selected avatar is displayed
        Assert.Equal(firstAvatar.Attributes["src"].Value, selectedAvatar.Attributes["src"].Value);
    }

    [Fact]
    public void SelectingAvatarUpdatesSessionAvatarUrl()
    {
        // Arrange
        var mockSession = new Mock<IStateContainer>();
        Services.AddSingleton(mockSession.Object);
        var component = RenderComponent<AvatarSelection>();
        var firstAvatar = component.Find("img.avatar");

        // Act
        firstAvatar.Click();

        // Assert
        mockSession.VerifySet(x => x.AvatarUrl = firstAvatar.Attributes["src"].Value, Times.Once);
    }
}
