using System.Threading.Tasks;
using Xunit;
using Moq;
using DeerskinSimulation.Commands;
using DeerskinSimulation.Models;
using DeerskinSimulation.ViewModels;

public class DeliverToCampCommandTests
{
    private readonly Mock<ISimulationViewModel> _mockViewModel;
    private readonly DeliverToCampCommand _command;
    private readonly RoleHunter _hunter;

    public DeliverToCampCommandTests()
    {
        _mockViewModel = new Mock<ISimulationViewModel>();
        _hunter = new RoleHunter("Hunter", 0, 0);
        _command = new DeliverToCampCommand(_mockViewModel.Object);
    }

    [Fact]
    public void CanExecute_ShouldReturnTrue()
    {
        // Act
        var canExecute = _command.CanExecute(null);

        // Assert
        Assert.True(canExecute);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldEndHuntAndClearCurrentBag()
    {
        // Arrange
        var mockActivityMeta = new TimelapseActivityMeta
        {
            Name = "Hunting",
            Duration = 5,
            Elapsed = 2
        };
        var mockUserActivity = new UserInitiatedActivitySequence
        {
            Meta = mockActivityMeta
        };

        var mockEventResult = new EventResult();
        mockEventResult.Records.Add(new EventRecord("End of hunt record"));

        _hunter.CurrentBag = 10; // Setting initial bag
        _mockViewModel.SetupGet(vm => vm.Hunter).Returns(_hunter);
        _mockViewModel.SetupGet(vm => vm.CurrentUserActivity).Returns(mockUserActivity);
        _mockViewModel.Setup(vm => vm.Hunter.EndHunt(It.IsAny<ISimulationViewModel>())).Returns(mockEventResult);

        // Act
        var resultStatus = await _command.ExecuteAsync();

        // Assert
        Assert.Equal(EventResultStatus.None, resultStatus);
        Assert.Equal(0, _mockViewModel.Object.Hunter.CurrentBag);
        _mockViewModel.Verify(vm => vm.AddMessage(It.IsAny<EventResult>()), Times.Once);
        _mockViewModel.Verify(vm => vm.SetFeatured(It.IsAny<EventRecord>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldAddMessageAndSetFeatured_WhenRecordsExist()
    {
        // Arrange
        var mockActivityMeta = new TimelapseActivityMeta
        {
            Name = "Hunting",
            Duration = 5,
            Elapsed = 2
        };
        var mockUserActivity = new UserInitiatedActivitySequence
        {
            Meta = mockActivityMeta
        };

        var mockEventResult = new EventResult();
        mockEventResult.Records.Add(new EventRecord("End of hunt record"));

        _mockViewModel.SetupGet(vm => vm.Hunter).Returns(_hunter);
        _mockViewModel.SetupGet(vm => vm.CurrentUserActivity).Returns(mockUserActivity);
        _mockViewModel.Setup(vm => vm.Hunter.EndHunt(It.IsAny<ISimulationViewModel>())).Returns(mockEventResult);

        // Act
        var resultStatus = await _command.ExecuteAsync();

        // Assert
        _mockViewModel.Verify(vm => vm.AddMessage(mockEventResult), Times.Once);
        _mockViewModel.Verify(vm => vm.SetFeatured(mockEventResult.LastRecord()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldNotAddMessageOrSetFeatured_WhenNoRecordsExist()
    {
        // Arrange
        var mockActivityMeta = new TimelapseActivityMeta
        {
            Name = "Hunting",
            Duration = 5,
            Elapsed = 2
        };
        var mockUserActivity = new UserInitiatedActivitySequence
        {
            Meta = mockActivityMeta
        };

        var mockEventResult = new EventResult(); // No records added

        _mockViewModel.SetupGet(vm => vm.Hunter).Returns(_hunter);
        _mockViewModel.SetupGet(vm => vm.CurrentUserActivity).Returns(mockUserActivity);
        _mockViewModel.Setup(vm => vm.Hunter.EndHunt(It.IsAny<ISimulationViewModel>())).Returns(mockEventResult);

        // Act
        var resultStatus = await _command.ExecuteAsync();

        // Assert
        _mockViewModel.Verify(vm => vm.AddMessage(It.IsAny<EventResult>()), Times.Never);
        _mockViewModel.Verify(vm => vm.SetFeatured(It.IsAny<EventRecord>()), Times.Never);
    }
}
