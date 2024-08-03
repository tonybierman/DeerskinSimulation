using Xunit;
using Moq;
using DeerskinSimulation.Commands;
using DeerskinSimulation.Models;
using DeerskinSimulation.ViewModels;
using System.Threading.Tasks;
using System.Net.Http;

public class TravelGreatWagonRoadCommandTests
{
    private readonly Mock<ISimulationViewModel> _mockViewModel;
    private readonly Mock<Trip> _mockJourney;
    private readonly TravelGreatWagonRoadCommand _command;
    private readonly RoleTrader _roleTrader;
    private readonly RoleExporter _roleExporter;

    public TravelGreatWagonRoadCommandTests()
    {
        // Initialize mocks
        _mockViewModel = new Mock<ISimulationViewModel>();
        _mockJourney = new Mock<Trip>(new Mock<HttpClient>().Object, "journey.json");

        // Create real instances for RoleTrader and RoleExporter
        _roleTrader = new RoleTrader("Trader", 1000, 100);
        _roleExporter = new RoleExporter("Exporter", 500, 0);

        // Setup the view model to return the trader and exporter
        _mockViewModel.Setup(vm => vm.Trader).Returns(_roleTrader);
        _mockViewModel.Setup(vm => vm.Exporter).Returns(_roleExporter);

        // Setup the CurrentUserActivity
        _mockViewModel.Setup(vm => vm.CurrentUserActivity).Returns(new UserInitiatedActivitySequence
        {
            Meta = new TimelapseActivityMeta { Name = "Journey", Duration = 10, Elapsed = 0 }
        });

        // Initialize the command
        _command = new TravelGreatWagonRoadCommand(_mockViewModel.Object, _mockJourney.Object, 10);
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
        eventResult.Records.Add(new EventRecord("Transported 10 skins."));
        _mockViewModel.Setup(vm => vm.Trader.TransportSkins(It.IsAny<ISimulationViewModel>(), It.IsAny<RoleExporter>(), It.IsAny<int>()))
                      .Returns(eventResult);

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
        _mockViewModel.Setup(vm => vm.Trader.TransportSkins(It.IsAny<ISimulationViewModel>(), It.IsAny<RoleExporter>(), It.IsAny<int>()))
                      .Returns(eventResult);

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
