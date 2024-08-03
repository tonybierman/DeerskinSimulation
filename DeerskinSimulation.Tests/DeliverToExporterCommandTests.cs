using Xunit;
using Moq;
using DeerskinSimulation.Commands;
using DeerskinSimulation.Models;
using DeerskinSimulation.ViewModels;
using System.Threading.Tasks;

public class DeliverToExporterCommandTests
{
    private readonly Mock<ISimulationViewModel> _mockViewModel;
    private readonly Mock<RoleTrader> _mockTrader;
    private readonly Mock<RoleExporter> _mockExporter;
    private readonly DeliverToExporterCommand _command;
    private const int NumberOfSkins = 10;

    public DeliverToExporterCommandTests()
    {
        // Set up mock dependencies
        _mockViewModel = new Mock<ISimulationViewModel>();
        _mockTrader = new Mock<RoleTrader>("Trader", 1000, 100, new Mock<IRandomEventStrategy>().Object);
        _mockExporter = new Mock<RoleExporter>("Exporter", 500, 50, new Mock<IRandomEventStrategy>().Object);

        // Set up the ISimulationViewModel to return mock trader and exporter
        _mockViewModel.Setup(v => v.Trader).Returns(_mockTrader.Object);
        _mockViewModel.Setup(v => v.Exporter).Returns(_mockExporter.Object);

        // Initialize the command with mocked view model and number of skins
        _command = new DeliverToExporterCommand(_mockViewModel.Object, NumberOfSkins);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldAddMessageAndSetFeatured_WhenRecordsExist()
    {
        // Arrange
        var resultWithRecords = new EventResult();
        resultWithRecords.Records.Add(new EventRecord("Delivered 10 skins to exporter.", "green"));
        resultWithRecords.Status = EventResultStatus.Success;

        _mockTrader.Setup(t => t.DeliverToExporter(_mockViewModel.Object, _mockExporter.Object, NumberOfSkins))
                   .Returns(resultWithRecords);

        // Act
        var resultStatus = await _command.ExecuteAsync();

        // Assert
        Assert.Equal(EventResultStatus.Success, resultStatus);
        _mockViewModel.Verify(v => v.SetFeatured(It.IsAny<EventRecord>()), Times.Once);
        _mockViewModel.Verify(v => v.AddMessage(It.IsAny<EventResult>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldNotAddMessageOrSetFeatured_WhenNoRecordsExist()
    {
        // Arrange
        var emptyResult = new EventResult(); // No records in this result

        _mockTrader.Setup(t => t.DeliverToExporter(_mockViewModel.Object, _mockExporter.Object, NumberOfSkins))
                   .Returns(emptyResult);

        // Act
        var resultStatus = await _command.ExecuteAsync();

        // Assert
        Assert.Equal(EventResultStatus.None, resultStatus);
        _mockViewModel.Verify(v => v.SetFeatured(It.IsAny<EventRecord>()), Times.Never);
        _mockViewModel.Verify(v => v.AddMessage(It.IsAny<EventResult>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenTraderLacksSkins()
    {
        // Arrange
        var failResult = new EventResult(new EventRecord("Not enough skins to deliver.", "red"))
        {
            Status = EventResultStatus.Fail
        };

        _mockTrader.Setup(t => t.DeliverToExporter(_mockViewModel.Object, _mockExporter.Object, NumberOfSkins))
                   .Returns(failResult);

        // Act
        var resultStatus = await _command.ExecuteAsync();

        // Assert
        Assert.Equal(EventResultStatus.Fail, resultStatus);
        _mockViewModel.Verify(v => v.SetFeatured(It.IsAny<EventRecord>()), Times.Once);
        _mockViewModel.Verify(v => v.AddMessage(It.IsAny<EventResult>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenExporterLacksFunds()
    {
        // Arrange
        _mockExporter.Setup(e => e.HasMoney(It.IsAny<double>())).Returns(false);

        var failResult = new EventResult(new EventRecord("Exporter does not have enough money to complete the transaction.", "red"))
        {
            Status = EventResultStatus.Fail
        };

        _mockTrader.Setup(t => t.DeliverToExporter(_mockViewModel.Object, _mockExporter.Object, NumberOfSkins))
                   .Returns(failResult);

        // Act
        var resultStatus = await _command.ExecuteAsync();

        // Assert
        Assert.Equal(EventResultStatus.Fail, resultStatus);
        _mockViewModel.Verify(v => v.SetFeatured(It.IsAny<EventRecord>()), Times.Once);
        _mockViewModel.Verify(v => v.AddMessage(It.IsAny<EventResult>()), Times.Once);
    }
}
