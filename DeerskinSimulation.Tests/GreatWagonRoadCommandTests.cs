using Xunit;
using Moq;
using DeerskinSimulation.Commands;
using DeerskinSimulation.Models;
using DeerskinSimulation.ViewModels;
using DeerskinSimulation.Resources;
using System.Threading.Tasks;

public class GreatWagonRoadCommandTests
{
    private readonly Mock<ISimulationViewModel> _mockViewModel;
    private readonly Mock<RoleTrader> _mockTrader;
    private readonly Mock<RoleExporter> _mockExporter;
    private readonly Mock<Trip> _mockTrip;
    private readonly GreatWagonRoadCommand _command;
    private readonly int _numberOfSkins;

    public GreatWagonRoadCommandTests()
    {
        _numberOfSkins = 10;

        // Mock ISimulationViewModel
        _mockViewModel = new Mock<ISimulationViewModel>();

        // Mock the RoleTrader and RoleExporter
        _mockTrader = new Mock<RoleTrader>("Trader", 1000, 100, new Mock<IRandomEventStrategy>().Object);
        _mockExporter = new Mock<RoleExporter>("Exporter", 500, 50, new Mock<IRandomEventStrategy>().Object);

        // Mock the Trip
        _mockTrip = new Mock<Trip>();

        // Setup the view model to return the mocked trader and exporter
        _mockViewModel.Setup(vm => vm.Trader).Returns(_mockTrader.Object);
        _mockViewModel.Setup(vm => vm.Exporter).Returns(_mockExporter.Object);
        _mockViewModel.Setup(vm => vm.CurrentUserActivity).Returns(new UserInitiatedActivitySequence
        {
            Meta = new TimelapseActivityMeta { Name = "Great Wagon Road", Duration = 10, Elapsed = 0 }
        });

        // Initialize the GreatWagonRoadCommand
        _command = new GreatWagonRoadCommand(_mockViewModel.Object, _mockTrip.Object, _numberOfSkins);
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
    public async Task ExecuteAsync_ShouldReturnSuccess_WhenTransportSuccessful()
    {
        // Arrange
        var eventResult = new EventResult { Status = EventResultStatus.Success };
        eventResult.Records.Add(new EventRecord("Transport successful."));
        _mockTrader.Setup(t => t.TransportSkins(It.IsAny<ISimulationViewModel>(), _mockExporter.Object, _numberOfSkins)).Returns(eventResult);

        // Act
        var status = await _command.ExecuteAsync();

        // Assert
        Assert.Equal(EventResultStatus.Success, status);
        _mockViewModel.Verify(vm => vm.AddMessage(It.Is<EventResult>(r => r.Status == EventResultStatus.Success)), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnFail_WhenTransportFails()
    {
        // Arrange
        var eventResult = new EventResult { Status = EventResultStatus.Fail };
        eventResult.Records.Add(new EventRecord("Transport failed."));
        _mockTrader.Setup(t => t.TransportSkins(It.IsAny<ISimulationViewModel>(), _mockExporter.Object, _numberOfSkins)).Returns(eventResult);

        // Act
        var status = await _command.ExecuteAsync();

        // Assert
        Assert.Equal(EventResultStatus.Fail, status);
        _mockViewModel.Verify(vm => vm.AddMessage(It.Is<EventResult>(r => r.Status == EventResultStatus.Fail)), Times.Once);
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
