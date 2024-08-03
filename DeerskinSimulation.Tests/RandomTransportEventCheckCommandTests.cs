using Xunit;
using Moq;
using DeerskinSimulation.Commands;
using DeerskinSimulation.Models;
using DeerskinSimulation.ViewModels;
using System.Threading.Tasks;

public class RandomTransportEventCheckCommandTests
{
    private readonly Mock<ISimulationViewModel> _mockViewModel;
    private readonly Mock<IRandomEventStrategy> _mockTransportingStrategy;
    private readonly RoleTrader _roleTrader;
    private readonly RandomTransportEventCheckCommand _command;

    public RandomTransportEventCheckCommandTests()
    {
        // Mock the view model
        _mockViewModel = new Mock<ISimulationViewModel>();

        // Mock the transporting strategy
        _mockTransportingStrategy = new Mock<IRandomEventStrategy>();

        // Create a RoleTrader with the mocked strategy
        _roleTrader = new RoleTrader("Trader", 1000, 100, _mockTransportingStrategy.Object);

        // Setup the view model to return the mocked trader
        _mockViewModel.Setup(vm => vm.Trader).Returns(_roleTrader);

        // Initialize the RandomTransportEventCheckCommand
        _command = new RandomTransportEventCheckCommand(_mockViewModel.Object);
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
        eventResult.Records.Add(new EventRecord("Random transporting event occurred."));
        _mockTransportingStrategy.Setup(t => t.ApplyEvent(It.IsAny<RoleTrader>())).Returns(eventResult);

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
        _mockTransportingStrategy.Setup(t => t.ApplyEvent(It.IsAny<RoleTrader>())).Returns(eventResult);

        // Act
        var status = await _command.ExecuteAsync();

        // Assert
        Assert.Equal(EventResultStatus.None, status);
        _mockViewModel.Verify(vm => vm.AddMessage(It.IsAny<EventResult>()), Times.Never);
        _mockViewModel.Verify(vm => vm.SetFeatured(It.IsAny<EventRecord>()), Times.Never);
    }
}
