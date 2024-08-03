using Xunit;
using Moq;
using System.Threading.Tasks;
using DeerskinSimulation.Commands;
using DeerskinSimulation.ViewModels;
using DeerskinSimulation.Models;
using DeerskinSimulation.Services;

public class ForwardingSequenceCoordinatorTests
{
    private readonly Mock<ISimulationViewModel> _mockViewModel;
    private readonly Mock<IGameLoopService> _mockGameLoopService;
    private readonly Mock<ICommandFactory> _mockCommandFactory;
    private readonly ForwardingSequenceCoordinator _coordinator;

    public ForwardingSequenceCoordinatorTests()
    {
        _mockViewModel = new Mock<ISimulationViewModel>();
        _mockGameLoopService = new Mock<IGameLoopService>();
        _mockCommandFactory = new Mock<ICommandFactory>();

        _coordinator = new ForwardingSequenceCoordinator(
            _mockViewModel.Object,
            _mockGameLoopService.Object,
            _mockCommandFactory.Object);

        // Initialize the mock properties to avoid null reference
        _mockViewModel.SetupProperty(vm => vm.CurrentUserActivity);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldStartActivity_WhenNumberOfSkinsIsGreaterThanZero()
    {
        // Arrange
        var sellOptions = new ForwardOptionsViewModel { NumberOfSkins = 10 };
        var mockWildernessRoadCommand = new Mock<WildernessRoadCommand>(_mockViewModel.Object);
        var mockRandomForwardingEventCheckCommand = new Mock<RandomForwardingEventCheckCommand>(_mockViewModel.Object);
        var mockForwardToTraderCommand = new Mock<ForwardToTraderCommand>(_mockViewModel.Object, sellOptions.NumberOfSkins);

        // Set up command factory to return mocks
        _mockCommandFactory.Setup(cf => cf.CreateWildernessRoadCommand(_mockViewModel.Object))
                           .Returns(mockWildernessRoadCommand.Object);
        _mockCommandFactory.Setup(cf => cf.CreateRandomForwardingEventCheckCommand(_mockViewModel.Object))
                           .Returns(mockRandomForwardingEventCheckCommand.Object);
        _mockCommandFactory.Setup(cf => cf.CreateForwardToTraderCommand(_mockViewModel.Object, sellOptions.NumberOfSkins))
                           .Returns(mockForwardToTraderCommand.Object);

        // Set up commands to do nothing on ExecuteAsync
        mockWildernessRoadCommand.Setup(cmd => cmd.ExecuteAsync()).ReturnsAsync(EventResultStatus.Success);
        mockRandomForwardingEventCheckCommand.Setup(cmd => cmd.ExecuteAsync()).ReturnsAsync(EventResultStatus.Success);
        mockForwardToTraderCommand.Setup(cmd => cmd.ExecuteAsync()).ReturnsAsync(EventResultStatus.Success);

        // Act
        await _coordinator.ExecuteAsync(sellOptions);

        // Assert
        Assert.NotNull(_mockViewModel.Object.CurrentUserActivity);
        Assert.Equal("Packtrain", _mockViewModel.Object.CurrentUserActivity.Meta.Name);
        _mockGameLoopService.Verify(gl => gl.StartActivity(It.IsAny<TimelapseActivityMeta>()), Times.Once);

        // Simulate the process execution
        await _mockViewModel.Object.CurrentUserActivity.InProcess.Invoke();
        await _mockViewModel.Object.CurrentUserActivity.Finish.Invoke();

        // Verify that commands were executed
        mockWildernessRoadCommand.Verify(cmd => cmd.ExecuteAsync(), Times.Once);
        mockRandomForwardingEventCheckCommand.Verify(cmd => cmd.ExecuteAsync(), Times.Once);
        mockForwardToTraderCommand.Verify(cmd => cmd.ExecuteAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldNotStartActivity_WhenNumberOfSkinsIsZero()
    {
        // Arrange
        var sellOptions = new ForwardOptionsViewModel { NumberOfSkins = 0 };

        // Act
        await _coordinator.ExecuteAsync(sellOptions);

        // Assert
        Assert.Null(_mockViewModel.Object.CurrentUserActivity);
        _mockGameLoopService.Verify(gl => gl.StartActivity(It.IsAny<TimelapseActivityMeta>()), Times.Never);
    }
}
