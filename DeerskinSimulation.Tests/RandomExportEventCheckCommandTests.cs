using Xunit;
using Moq;
using DeerskinSimulation.Commands;
using DeerskinSimulation.Models;
using DeerskinSimulation.ViewModels;
using System.Threading.Tasks;

public class RandomExportEventCheckCommandTests
{
    private readonly Mock<ISimulationViewModel> _mockViewModel;
    private readonly Mock<RoleExporter> _mockExporter;
    private readonly RandomExportEventCheckCommand _command;

    public RandomExportEventCheckCommandTests()
    {
        // Mock ISimulationViewModel
        _mockViewModel = new Mock<ISimulationViewModel>();

        // Mock the RoleExporter
        _mockExporter = new Mock<RoleExporter>("Exporter", 1000, 100, new Mock<IRandomEventStrategy>().Object);

        // Setup the view model to return the mocked exporter
        _mockViewModel.Setup(vm => vm.Exporter).Returns(_mockExporter.Object);

        // Initialize the RandomExportEventCheckCommand
        _command = new RandomExportEventCheckCommand(_mockViewModel.Object);
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
        eventResult.Records.Add(new EventRecord("Random export event occurred."));
        _mockExporter.Setup(e => e.RollForRandomExportingEvent()).Returns(eventResult);

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
        _mockExporter.Setup(e => e.RollForRandomExportingEvent()).Returns(eventResult);

        // Act
        var status = await _command.ExecuteAsync();

        // Assert
        Assert.Equal(EventResultStatus.None, status);
        _mockViewModel.Verify(vm => vm.AddMessage(It.IsAny<EventResult>()), Times.Never);
        _mockViewModel.Verify(vm => vm.SetFeatured(It.IsAny<EventRecord>()), Times.Never);
    }
}
