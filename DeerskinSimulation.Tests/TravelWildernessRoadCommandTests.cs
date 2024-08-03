using Xunit;
using Moq;
using DeerskinSimulation.Commands;
using DeerskinSimulation.Models;
using DeerskinSimulation.ViewModels;
using System;
using System.Threading.Tasks;

public class TravelWildernessRoadCommandTests
{
    private readonly Mock<ISimulationViewModel> _mockViewModel;
    private readonly TravelWildernessRoadCommand _command;
    private readonly RoleHunter _roleHunter;

    public TravelWildernessRoadCommandTests()
    {
        // Initialize mocks
        _mockViewModel = new Mock<ISimulationViewModel>();

        // Create real instance for RoleHunter
        _roleHunter = new RoleHunter("Hunter", 1000, 100);

        // Setup the view model to return the hunter
        _mockViewModel.Setup(vm => vm.Hunter).Returns(_roleHunter);

        // Setup the CurrentUserActivity
        _mockViewModel.Setup(vm => vm.CurrentUserActivity).Returns(new UserInitiatedActivitySequence
        {
            Meta = new TimelapseActivityMeta { Name = "Journey", Duration = 10, Elapsed = 0 }
        });

        // Initialize the command
        _command = new TravelWildernessRoadCommand(_mockViewModel.Object);
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
    public async Task ExecuteAsync_ShouldAddMessage_WhenEventHasRecords()
    {
        // Arrange
        var eventResult = new EventResult { Status = EventResultStatus.Success };
        eventResult.Records.Add(new EventRecord("Traveled 20 miles."));
        _mockViewModel.Setup(vm => vm.Hunter.Travel(It.IsAny<ISimulationViewModel>())).Returns(eventResult);

        // Act
        var status = await _command.ExecuteAsync();

        // Assert
        Assert.Equal(EventResultStatus.Success, status);
        _mockViewModel.Verify(vm => vm.AddMessage(It.Is<EventResult>(r => r.Status == EventResultStatus.Success)), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldNotAddMessage_WhenEventHasNoRecords()
    {
        // Arrange
        var eventResult = new EventResult { Status = EventResultStatus.None };
        _mockViewModel.Setup(vm => vm.Hunter.Travel(It.IsAny<ISimulationViewModel>())).Returns(eventResult);

        // Act
        var status = await _command.ExecuteAsync();

        // Assert
        Assert.Equal(EventResultStatus.None, status);
        _mockViewModel.Verify(vm => vm.AddMessage(It.IsAny<EventResult>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowException_WhenUserActivityMetaIsNull()
    {
        // Arrange
        _mockViewModel.Setup(vm => vm.CurrentUserActivity).Returns((UserInitiatedActivitySequence)null);

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => _command.ExecuteAsync());
    }
}
