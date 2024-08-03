using Xunit;
using Moq;
using DeerskinSimulation.Commands;
using DeerskinSimulation.Models;
using DeerskinSimulation.ViewModels;
using DeerskinSimulation.Resources;
using System.Threading.Tasks;

public class ForwardToTraderCommandTests
{
    private readonly Mock<ISimulationViewModel> _mockViewModel;
    private readonly Mock<RoleHunter> _mockHunter;
    private readonly Mock<RoleTrader> _mockTrader;
    private readonly ForwardToTraderCommand _command;
    private readonly int _numberOfSkins;

    public ForwardToTraderCommandTests()
    {
        _numberOfSkins = 10;

        // Mock ISimulationViewModel
        _mockViewModel = new Mock<ISimulationViewModel>();

        // Mock the RoleHunter and RoleTrader
        _mockHunter = new Mock<RoleHunter>("Hunter", 1000, 100, new Mock<IRandomEventStrategy>().Object, new Mock<IRandomEventStrategy>().Object);
        _mockTrader = new Mock<RoleTrader>("Trader", 500, 50, new Mock<IRandomEventStrategy>().Object);

        // Setup the view model to return the mocked hunter and trader
        _mockViewModel.Setup(vm => vm.Hunter).Returns(_mockHunter.Object);
        _mockViewModel.Setup(vm => vm.Trader).Returns(_mockTrader.Object);

        // Initialize the ForwardToTraderCommand
        _command = new ForwardToTraderCommand(_mockViewModel.Object, _numberOfSkins);
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
    public async Task ExecuteAsync_ShouldReturnSuccess_WhenDeliverySuccessful()
    {
        // Arrange
        var eventResult = new EventResult { Status = EventResultStatus.Success };
        eventResult.Records.Add(new EventRecord("Delivery successful."));
        _mockHunter.Setup(h => h.DeliverToTrader(It.IsAny<RoleTrader>(), _numberOfSkins)).Returns(eventResult);

        // Act
        var status = await _command.ExecuteAsync();

        // Assert
        Assert.Equal(EventResultStatus.Success, status);
        _mockViewModel.Verify(vm => vm.AddMessage(It.Is<EventResult>(r => r.Status == EventResultStatus.Success)), Times.Once);
        _mockViewModel.Verify(vm => vm.SetFeatured(It.IsAny<EventRecord>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnFail_WhenInsufficientSkins()
    {
        // Arrange
        var eventResult = new EventResult { Status = EventResultStatus.Fail };
        eventResult.Records.Add(new EventRecord("Not enough skins."));
        _mockHunter.Setup(h => h.DeliverToTrader(It.IsAny<RoleTrader>(), _numberOfSkins)).Returns(eventResult);

        // Act
        var status = await _command.ExecuteAsync();

        // Assert
        Assert.Equal(EventResultStatus.Fail, status);
        _mockViewModel.Verify(vm => vm.AddMessage(It.Is<EventResult>(r => r.Status == EventResultStatus.Fail)), Times.Once);
        _mockViewModel.Verify(vm => vm.SetFeatured(It.IsAny<EventRecord>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldNotAddMessageOrSetFeatured_WhenNoRecordsExist()
    {
        // Arrange
        var eventResult = new EventResult { Status = EventResultStatus.Success }; // No records added
        _mockHunter.Setup(h => h.DeliverToTrader(It.IsAny<RoleTrader>(), _numberOfSkins)).Returns(eventResult);

        // Act
        var status = await _command.ExecuteAsync();

        // Assert
        Assert.Equal(EventResultStatus.Success, status);
        _mockViewModel.Verify(vm => vm.AddMessage(It.IsAny<EventResult>()), Times.Never);
        _mockViewModel.Verify(vm => vm.SetFeatured(It.IsAny<EventRecord>()), Times.Never);
    }
}
