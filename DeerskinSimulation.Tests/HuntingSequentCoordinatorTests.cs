using Xunit;
using Moq;
using System.Threading.Tasks;
using DeerskinSimulation.Commands;
using DeerskinSimulation.ViewModels;
using DeerskinSimulation.Models;
using DeerskinSimulation.Services;
using DeerskinSimulation.Resources;

public class HuntingSequentCoordinatorTests
{
    private readonly Mock<ISimulationViewModel> _mockViewModel;
    private readonly Mock<IGameLoopService> _mockGameLoopService;
    private readonly Mock<ICommandFactory> _mockCommandFactory;
    private readonly HuntingSequentCoordinator _coordinator;

    public HuntingSequentCoordinatorTests()
    {
        _mockViewModel = new Mock<ISimulationViewModel>();
        _mockGameLoopService = new Mock<IGameLoopService>();
        _mockCommandFactory = new Mock<ICommandFactory>();

        _coordinator = new HuntingSequentCoordinator(
            _mockViewModel.Object,
            _mockGameLoopService.Object,
            _mockCommandFactory.Object);

        _mockViewModel.SetupProperty(vm => vm.CurrentUserActivity);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldStartActivity()
    {
        // Arrange
        var huntOptions = new HuntOptionsViewModel();
        var mockHuntCommand = new Mock<HuntCommand>(_mockViewModel.Object);
        var mockRandomHuntingEventCheckCommand = new Mock<RandomHuntingEventCheckCommand>(_mockViewModel.Object);
        var mockDeliverToCampCommand = new Mock<DeliverToCampCommand>(_mockViewModel.Object);

        _mockCommandFactory.Setup(cf => cf.CreateHuntCommand(_mockViewModel.Object))
                           .Returns(mockHuntCommand.Object);
        _mockCommandFactory.Setup(cf => cf.CreateRandomHuntingEventCheckCommand(_mockViewModel.Object))
                           .Returns(mockRandomHuntingEventCheckCommand.Object);
        _mockCommandFactory.Setup(cf => cf.CreateDeliverToCampCommand(_mockViewModel.Object))
                           .Returns(mockDeliverToCampCommand.Object);

        mockHuntCommand.Setup(cmd => cmd.ExecuteAsync()).ReturnsAsync(EventResultStatus.Success);
        mockRandomHuntingEventCheckCommand.Setup(cmd => cmd.ExecuteAsync()).ReturnsAsync(EventResultStatus.None);
        mockDeliverToCampCommand.Setup(cmd => cmd.ExecuteAsync()).ReturnsAsync(EventResultStatus.Success);

        // Act
        await _coordinator.ExecuteAsync(huntOptions);

        // Assert
        Assert.NotNull(_mockViewModel.Object.CurrentUserActivity);
        Assert.Equal(Strings.HuntingActivityName, _mockViewModel.Object.CurrentUserActivity.Meta.Name);
        _mockGameLoopService.Verify(gl => gl.StartActivity(It.IsAny<TimelapseActivityMeta>()), Times.Once);

        // Simulate process execution
        await _mockViewModel.Object.CurrentUserActivity.InProcess.Invoke();
        await _mockViewModel.Object.CurrentUserActivity.Finish.Invoke();

        mockHuntCommand.Verify(cmd => cmd.ExecuteAsync(), Times.Once);
        mockRandomHuntingEventCheckCommand.Verify(cmd => cmd.ExecuteAsync(), Times.Once);
        mockDeliverToCampCommand.Verify(cmd => cmd.ExecuteAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldSetStatusFail_WhenHuntFails()
    {
        // Arrange
        var huntOptions = new HuntOptionsViewModel();
        var mockHuntCommand = new Mock<HuntCommand>(_mockViewModel.Object);
        var mockRandomHuntingEventCheckCommand = new Mock<RandomHuntingEventCheckCommand>(_mockViewModel.Object);
        var mockDeliverToCampCommand = new Mock<DeliverToCampCommand>(_mockViewModel.Object);

        _mockCommandFactory.Setup(cf => cf.CreateHuntCommand(_mockViewModel.Object))
                           .Returns(mockHuntCommand.Object);
        _mockCommandFactory.Setup(cf => cf.CreateRandomHuntingEventCheckCommand(_mockViewModel.Object))
                           .Returns(mockRandomHuntingEventCheckCommand.Object);
        _mockCommandFactory.Setup(cf => cf.CreateDeliverToCampCommand(_mockViewModel.Object))
                           .Returns(mockDeliverToCampCommand.Object);

        mockHuntCommand.Setup(cmd => cmd.ExecuteAsync()).ReturnsAsync(EventResultStatus.Fail);
        mockRandomHuntingEventCheckCommand.Setup(cmd => cmd.ExecuteAsync()).ReturnsAsync(EventResultStatus.None);
        mockDeliverToCampCommand.Setup(cmd => cmd.ExecuteAsync()).ReturnsAsync(EventResultStatus.Success);

        // Act
        await _coordinator.ExecuteAsync(huntOptions);

        // Assert
        Assert.NotNull(_mockViewModel.Object.CurrentUserActivity);
        Assert.Equal(Strings.HuntingActivityName, _mockViewModel.Object.CurrentUserActivity.Meta.Name);

        // Simulate process execution
        await _mockViewModel.Object.CurrentUserActivity.InProcess.Invoke();

        Assert.Equal(EventResultStatus.Fail, _mockViewModel.Object.CurrentUserActivity.Meta.Status);

        // Verify that random hunting event check command was never executed due to failure
        mockHuntCommand.Verify(cmd => cmd.ExecuteAsync(), Times.Once);
        // TODO: mockRandomHuntingEventCheckCommand.Verify(cmd => cmd.ExecuteAsync(), Times.Never);

        // Simulate finishing the process regardless of the status
        await _mockViewModel.Object.CurrentUserActivity.Finish.Invoke();
        // TODO: mockDeliverToCampCommand.Verify(cmd => cmd.ExecuteAsync(), Times.Once);
    }
}
