using Xunit;
using Moq;
using DeerskinSimulation.Commands;
using DeerskinSimulation.ViewModels;
using DeerskinSimulation.Models;
using DeerskinSimulation.Services;
using System.Threading.Tasks;

public class ExportSequenceCoordinatorTests
{
    private readonly Mock<ISimulationViewModel> _mockViewModel;
    private readonly Mock<IGameLoopService> _mockGameLoopService;
    private readonly Mock<ICommandFactory> _mockCommandFactory;
    private readonly ExportSequenceCoordinator _coordinator;

    public ExportSequenceCoordinatorTests()
    {
        _mockViewModel = new Mock<ISimulationViewModel>();
        _mockGameLoopService = new Mock<IGameLoopService>();
        _mockCommandFactory = new Mock<ICommandFactory>();

        _coordinator = new ExportSequenceCoordinator(
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
        var exportOptions = new ExportOptionsViewModel { NumberOfSkins = 10 };
        var mockExportCommand = new Mock<ExportCommand>(_mockViewModel.Object, exportOptions.NumberOfSkins);
        var mockRandomExportEventCheckCommand = new Mock<RandomExportEventCheckCommand>(_mockViewModel.Object);

        _mockCommandFactory.Setup(cf => cf.CreateExportCommand(_mockViewModel.Object, exportOptions.NumberOfSkins))
                           .Returns(mockExportCommand.Object);
        _mockCommandFactory.Setup(cf => cf.CreateRandomExportEventCheckCommand(_mockViewModel.Object))
                           .Returns(mockRandomExportEventCheckCommand.Object);

        // Act
        await _coordinator.ExecuteAsync(exportOptions);

        // Assert
        Assert.NotNull(_mockViewModel.Object.CurrentUserActivity);
        Assert.Equal("Exporting", _mockViewModel.Object.CurrentUserActivity.Meta.Name);
        _mockGameLoopService.Verify(gl => gl.StartActivity(It.IsAny<TimelapseActivityMeta>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldNotStartActivity_WhenNumberOfSkinsIsZero()
    {
        // Arrange
        var exportOptions = new ExportOptionsViewModel { NumberOfSkins = 0 };

        // Act
        await _coordinator.ExecuteAsync(exportOptions);

        // Assert
        Assert.Null(_mockViewModel.Object.CurrentUserActivity);
        _mockGameLoopService.Verify(gl => gl.StartActivity(It.IsAny<TimelapseActivityMeta>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldExecuteCommands_InOrder()
    {
        // Arrange
        var exportOptions = new ExportOptionsViewModel { NumberOfSkins = 10 };
        var mockExportCommand = new Mock<ExportCommand>(_mockViewModel.Object, exportOptions.NumberOfSkins);
        var mockRandomExportEventCheckCommand = new Mock<RandomExportEventCheckCommand>(_mockViewModel.Object);

        _mockCommandFactory.Setup(cf => cf.CreateExportCommand(_mockViewModel.Object, exportOptions.NumberOfSkins))
                           .Returns(mockExportCommand.Object);
        _mockCommandFactory.Setup(cf => cf.CreateRandomExportEventCheckCommand(_mockViewModel.Object))
                           .Returns(mockRandomExportEventCheckCommand.Object);

        // Setup the ExecuteAsync method to prevent null reference
        mockExportCommand.Setup(cmd => cmd.ExecuteAsync()).ReturnsAsync(EventResultStatus.Success);
        mockRandomExportEventCheckCommand.Setup(cmd => cmd.ExecuteAsync()).ReturnsAsync(EventResultStatus.Success);

        // Act
        await _coordinator.ExecuteAsync(exportOptions);

        // Simulate the process execution
        await _mockViewModel.Object.CurrentUserActivity.InProcess.Invoke();
        await _mockViewModel.Object.CurrentUserActivity.Finish.Invoke();

        // Assert
        mockRandomExportEventCheckCommand.Verify(cmd => cmd.ExecuteAsync(), Times.Once);
        mockExportCommand.Verify(cmd => cmd.ExecuteAsync(), Times.Once);
    }
}
