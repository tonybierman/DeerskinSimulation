using Xunit;
using Moq;
using DeerskinSimulation.Commands;
using DeerskinSimulation.Models;
using DeerskinSimulation.ViewModels;
using System.Threading.Tasks;

public class HuntCommandTests
{
    private readonly Mock<ISimulationViewModel> _mockViewModel;
    private readonly Mock<RoleHunter> _mockHunter;
    private readonly HuntCommand _command;

    public HuntCommandTests()
    {
        // Mock ISimulationViewModel
        _mockViewModel = new Mock<ISimulationViewModel>();

        // Mock the RoleHunter
        _mockHunter = new Mock<RoleHunter>("Hunter", 1000, 100, new Mock<IRandomEventStrategy>().Object, new Mock<IRandomEventStrategy>().Object);

        // Setup the view model to return the mocked hunter
        _mockViewModel.Setup(vm => vm.Hunter).Returns(_mockHunter.Object);
        _mockViewModel.Setup(vm => vm.CurrentUserActivity).Returns(new UserInitiatedActivitySequence
        {
            Meta = new TimelapseActivityMeta { Name = "Hunting", Duration = 10, Elapsed = 0 }
        });

        // Initialize the HuntCommand
        _command = new HuntCommand(_mockViewModel.Object);
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
    public async Task ExecuteAsync_ShouldReturnSuccess_WhenHuntSuccessful()
    {
        // Arrange
        var eventResult = new EventResult { Status = EventResultStatus.Success };
        eventResult.Records.Add(new EventRecord("Hunt successful."));
        _mockHunter.Setup(h => h.Hunt(It.IsAny<ISimulationViewModel>())).Returns(eventResult);

        // Act
        var status = await _command.ExecuteAsync();

        // Assert
        Assert.Equal(EventResultStatus.Success, status);
        _mockViewModel.Verify(vm => vm.AddMessage(It.Is<EventResult>(r => r.Status == EventResultStatus.Success)), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnFail_WhenHuntFails()
    {
        // Arrange
        var eventResult = new EventResult { Status = EventResultStatus.Fail };
        eventResult.Records.Add(new EventRecord("Hunt failed."));
        _mockHunter.Setup(h => h.Hunt(It.IsAny<ISimulationViewModel>())).Returns(eventResult);

        // Act
        var status = await _command.ExecuteAsync();

        // Assert
        Assert.Equal(EventResultStatus.Fail, status);
        _mockViewModel.Verify(vm => vm.AddMessage(It.Is<EventResult>(r => r.Status == EventResultStatus.Fail)), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldHandleNullCurrentUserActivity()
    {
        // Arrange
        _mockViewModel.Setup(vm => vm.CurrentUserActivity).Returns((UserInitiatedActivitySequence)null);

        var eventResult = new EventResult { Status = EventResultStatus.Success };
        eventResult.Records.Add(new EventRecord("Hunt successful without user activity."));
        _mockHunter.Setup(h => h.Hunt(It.IsAny<ISimulationViewModel>())).Returns(eventResult);

        // Act
        var status = await _command.ExecuteAsync();

        // Assert
        Assert.Equal(EventResultStatus.Success, status);
        _mockViewModel.Verify(vm => vm.AddMessage(It.Is<EventResult>(r => r.Status == EventResultStatus.Success)), Times.Once);
    }
}
