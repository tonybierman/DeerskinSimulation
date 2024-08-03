using Xunit;
using Moq;
using DeerskinSimulation.Commands;
using DeerskinSimulation.Models;
using DeerskinSimulation.ViewModels;
using System.Threading.Tasks;

public class RandomHuntingEventCheckCommandTests
{
    private readonly Mock<ISimulationViewModel> _mockViewModel;
    private readonly Mock<IRandomEventStrategy> _mockHuntingStrategy;
    private readonly RoleHunter _roleHunter;
    private readonly RandomHuntingEventCheckCommand _command;

    public RandomHuntingEventCheckCommandTests()
    {
        // Mock the view model
        _mockViewModel = new Mock<ISimulationViewModel>();

        // Mock the hunting strategy
        _mockHuntingStrategy = new Mock<IRandomEventStrategy>();

        // Create a RoleHunter with the mocked strategy
        _roleHunter = new RoleHunter("Hunter", 1000, 100, _mockHuntingStrategy.Object, null);

        // Setup the view model to return the mocked hunter
        _mockViewModel.Setup(vm => vm.Hunter).Returns(_roleHunter);

        // Initialize the RandomHuntingEventCheckCommand
        _command = new RandomHuntingEventCheckCommand(_mockViewModel.Object);
    }

    [Fact]
    public void CanExecute_ShouldAlwaysReturnTrue()
    {
        // Act
        var canExecute = _command.CanExecute(null);

        // Assert
        Assert.True(canExecute);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldAddMessageAndSetFeatured_WhenEventHasRecords()
    {
        // Arrange
        var eventResult = new EventResult { Status = EventResultStatus.Success };
        eventResult.Records.Add(new EventRecord("Random hunting event occurred."));
        _mockHuntingStrategy.Setup(h => h.ApplyEvent(It.IsAny<RoleHunter>())).Returns(eventResult);

        // Act
        var status = await _command.ExecuteAsync();

        // Assert
        Assert.Equal(EventResultStatus.Success, status);
        _mockViewModel.Verify(vm => vm.AddMessage(It.Is<EventResult>(r => r.Status == EventResultStatus.Success)), Times.Once);
        _mockViewModel.Verify(vm => vm.SetFeatured(It.IsAny<EventRecord>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldNotAddMessageOrSetFeatured_WhenEventHasNoRecords()
    {
        // Arrange
        var eventResult = new EventResult { Status = EventResultStatus.None };
        _mockHuntingStrategy.Setup(h => h.ApplyEvent(It.IsAny<RoleHunter>())).Returns(eventResult);

        // Act
        var status = await _command.ExecuteAsync();

        // Assert
        Assert.Equal(EventResultStatus.None, status);
        _mockViewModel.Verify(vm => vm.AddMessage(It.IsAny<EventResult>()), Times.Never);
        _mockViewModel.Verify(vm => vm.SetFeatured(It.IsAny<EventRecord>()), Times.Never);
    }
}
