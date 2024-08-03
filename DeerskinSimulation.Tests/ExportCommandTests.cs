using Xunit;
using Moq;
using DeerskinSimulation.Commands;
using DeerskinSimulation.Models;
using DeerskinSimulation.ViewModels;
using DeerskinSimulation.Resources;

public class ExportCommandTests
{
    private readonly Mock<ISimulationViewModel> _mockViewModel;
    private readonly Mock<RoleExporter> _mockExporter;
    private readonly ExportCommand _command;
    private readonly int _numberOfSkins;

    public ExportCommandTests()
    {
        _numberOfSkins = 10;

        // Mock the ISimulationViewModel
        _mockViewModel = new Mock<ISimulationViewModel>();

        // Mock the RoleExporter with default behavior
        _mockExporter = new Mock<RoleExporter>("Exporter", 1000, 50, new Mock<IRandomEventStrategy>().Object);

        // Setup the view model to return the mocked exporter
        _mockViewModel.Setup(vm => vm.Exporter).Returns(_mockExporter.Object);

        // Initialize the ExportCommand
        _command = new ExportCommand(_mockViewModel.Object, _numberOfSkins);
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
    public async Task ExecuteAsync_ShouldReturnSuccess_WhenExportIsSuccessful()
    {
        // Arrange
        var eventResult = new EventResult { Status = EventResultStatus.Success };
        eventResult.Records.Add(new EventRecord("Exported successfully."));

        _mockExporter.Setup(e => e.Export(It.IsAny<int>())).Returns(eventResult);

        // Act
        var status = await _command.ExecuteAsync();

        // Assert
        Assert.Equal(EventResultStatus.Success, status);
        _mockViewModel.Verify(vm => vm.AddMessage(It.Is<EventResult>(r => r.Status == EventResultStatus.Success)), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnFail_WhenExportFails()
    {
        // Arrange
        var eventResult = new EventResult { Status = EventResultStatus.Fail };
        eventResult.Records.Add(new EventRecord("Export failed."));

        _mockExporter.Setup(e => e.Export(It.IsAny<int>())).Returns(eventResult);

        // Act
        var status = await _command.ExecuteAsync();

        // Assert
        Assert.Equal(EventResultStatus.Fail, status);
        _mockViewModel.Verify(vm => vm.AddMessage(It.Is<EventResult>(r => r.Status == EventResultStatus.Fail)), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldNotAddMessage_WhenNoRecordsExist()
    {
        // Arrange
        var eventResult = new EventResult { Status = EventResultStatus.Success }; // No records added
        _mockExporter.Setup(e => e.Export(It.IsAny<int>())).Returns(eventResult);

        // Act
        var status = await _command.ExecuteAsync();

        // Assert
        Assert.Equal(EventResultStatus.Success, status);
        _mockViewModel.Verify(vm => vm.AddMessage(It.IsAny<EventResult>()), Times.Never);
    }
}
