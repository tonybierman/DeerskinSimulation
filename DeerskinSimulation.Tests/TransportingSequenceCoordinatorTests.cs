using Xunit;
using Moq;
using System.Threading.Tasks;
using DeerskinSimulation.Commands;
using DeerskinSimulation.ViewModels;
using DeerskinSimulation.Models;
using DeerskinSimulation.Services;

public class TransportingSequenceCoordinatorTests
{
    private readonly Mock<ISimulationViewModel> _mockViewModel;
    private readonly Mock<IGameLoopService> _mockGameLoopService;
    private readonly Mock<ICommandFactory> _mockCommandFactory;
    private readonly TransportingSequenceCoordinator _coordinator;

    public TransportingSequenceCoordinatorTests()
    {
        _mockViewModel = new Mock<ISimulationViewModel>();
        _mockGameLoopService = new Mock<IGameLoopService>();
        _mockCommandFactory = new Mock<ICommandFactory>();

        _coordinator = new TransportingSequenceCoordinator(
            _mockViewModel.Object,
            _mockGameLoopService.Object,
            _mockCommandFactory.Object
        );

        _mockViewModel.SetupProperty(vm => vm.CurrentUserActivity);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldNotStartActivity_WhenNumberOfSkinsIsZero()
    {
        // Arrange
        var transportOptions = new TransportOptionsViewModel { NumberOfSkins = 0 };

        // Act
        await _coordinator.ExecuteAsync(transportOptions);

        // Assert
        _mockGameLoopService.Verify(gl => gl.StartActivity(It.IsAny<TimelapseActivityMeta>()), Times.Never);
        Assert.Null(_mockViewModel.Object.CurrentUserActivity);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldStartActivity_WhenNumberOfSkinsIsGreaterThanZero()
    {
        // Arrange
        var transportOptions = new TransportOptionsViewModel { NumberOfSkins = 10 };
        var mockTravelCommand = new Mock<GreatWagonRoadCommand>(_mockViewModel.Object, null, transportOptions.NumberOfSkins);
        var mockDeliverToExporterCommand = new Mock<DeliverToExporterCommand>(_mockViewModel.Object, transportOptions.NumberOfSkins);
        var mockRandomTransportEventCheckCommand = new Mock<RandomTransportEventCheckCommand>(_mockViewModel.Object);

        _mockCommandFactory.Setup(cf => cf.CreateGreatWagonRoadCommand(It.IsAny<ISimulationViewModel>(), It.IsAny<Trip>(), It.IsAny<int>()))
                           .Returns(mockTravelCommand.Object);
        _mockCommandFactory.Setup(cf => cf.CreateDeliverToExporterCommand(It.IsAny<ISimulationViewModel>(), It.IsAny<int>()))
                           .Returns(mockDeliverToExporterCommand.Object);
        _mockCommandFactory.Setup(cf => cf.CreateRandomTransportEventCheckCommand(It.IsAny<ISimulationViewModel>()))
                           .Returns(mockRandomTransportEventCheckCommand.Object);

        mockTravelCommand.Setup(cmd => cmd.ExecuteAsync()).ReturnsAsync(EventResultStatus.Success);
        mockDeliverToExporterCommand.Setup(cmd => cmd.ExecuteAsync()).ReturnsAsync(EventResultStatus.Success);
        mockRandomTransportEventCheckCommand.Setup(cmd => cmd.ExecuteAsync()).ReturnsAsync(EventResultStatus.None);

        // Act
        await _coordinator.ExecuteAsync(transportOptions);

        // Assert
        Assert.NotNull(_mockViewModel.Object.CurrentUserActivity);
        Assert.Equal("Wagontrain", _mockViewModel.Object.CurrentUserActivity.Meta.Name);
        _mockGameLoopService.Verify(gl => gl.StartActivity(It.IsAny<TimelapseActivityMeta>()), Times.Once);

        // Simulate process execution
        await _mockViewModel.Object.CurrentUserActivity.InProcess.Invoke();
        await _mockViewModel.Object.CurrentUserActivity.Finish.Invoke();

        mockTravelCommand.Verify(cmd => cmd.ExecuteAsync(), Times.Once);
        mockRandomTransportEventCheckCommand.Verify(cmd => cmd.ExecuteAsync(), Times.Once);
        mockDeliverToExporterCommand.Verify(cmd => cmd.ExecuteAsync(), Times.Once);
    }
}
