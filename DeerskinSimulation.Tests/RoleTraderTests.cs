using Xunit;
using Moq;
using DeerskinSimulation.Models;
using DeerskinSimulation.ViewModels;
using DeerskinSimulation.Resources;
using System.Collections.Generic;

public class RoleTraderTests
{
    private readonly RoleTrader _roleTrader;
    private readonly RoleExporter _exporter;
    private readonly Mock<ISimulationViewModel> _mockViewModel;
    private readonly Mock<IRandomEventStrategy> _mockEventStrategy;

    public RoleTraderTests()
    {
        // Initialize the RoleTrader and RoleExporter
        _mockEventStrategy = new Mock<IRandomEventStrategy>();
        _roleTrader = new RoleTrader("Trader", 1000, 100, _mockEventStrategy.Object);
        _exporter = new RoleExporter("Exporter", 500, 50, new Mock<IRandomEventStrategy>().Object);

        // Mocking ISimulationViewModel
        _mockViewModel = new Mock<ISimulationViewModel>();
    }

    [Fact]
    public void DeliverToExporter_ShouldFail_WhenExporterHasNotEnoughMoney()
    {
        // Arrange
        _exporter.RemoveMoney(_exporter.Money); // Ensure exporter has no money

        // Act
        var result = _roleTrader.DeliverToExporter(_mockViewModel.Object, _exporter, 10);

        // Assert
        Assert.Equal(EventResultStatus.Fail, result.Status);
        Assert.Contains("Exporter does not have enough money", result.Records[0].Message);
    }

    [Fact]
    public void DeliverToExporter_ShouldFail_WhenNotEnoughSkins()
    {
        // Arrange
        _roleTrader.RemoveSkins(_roleTrader.Skins); // Ensure trader has no skins

        // Act
        var result = _roleTrader.DeliverToExporter(_mockViewModel.Object, _exporter, 10);

        // Assert
        Assert.Equal(EventResultStatus.Fail, result.Status);
        Assert.Contains("Not enough skins to deliver", result.Records[0].Message);
    }

    [Fact]
    public void DeliverToExporter_ShouldSucceed_WhenEnoughResources()
    {
        // Act
        var result = _roleTrader.DeliverToExporter(_mockViewModel.Object, _exporter, 10);

        // Assert
        Assert.Equal(EventResultStatus.Success, result.Status);
        Assert.Contains("Delivered 10 skins to exporter", result.Records[0].Message);
    }

    [Fact]
    public void TransportSkins_ShouldFail_WhenNotEnoughMoney()
    {
        // Arrange
        _roleTrader.RemoveMoney(_roleTrader.Money); // Ensure trader has no money

        _mockViewModel.Setup(vm => vm.CurrentUserActivity).Returns(new UserInitiatedActivitySequence
        {
            Meta = new TimelapseActivityMeta { Name = "Transporting", Duration = 10, Elapsed = 0 }
        });

        // Act
        var result = _roleTrader.TransportSkins(_mockViewModel.Object, _exporter, 10);

        // Assert
        Assert.Equal(EventResultStatus.Fail, result.Status);
        Assert.Contains(Strings.NotEnoughMoneyTravel, result.Records[0].Message);
    }

    [Fact]
    public void TransportSkins_ShouldFail_WhenNotEnoughSkins()
    {
        // Arrange
        _roleTrader.RemoveSkins(_roleTrader.Skins); // Ensure trader has no skins

        _mockViewModel.Setup(vm => vm.CurrentUserActivity).Returns(new UserInitiatedActivitySequence
        {
            Meta = new TimelapseActivityMeta { Name = "Transporting", Duration = 10, Elapsed = 0 }
        });

        // Act
        var result = _roleTrader.TransportSkins(_mockViewModel.Object, _exporter, 10);

        // Assert
        Assert.Equal(EventResultStatus.Fail, result.Status);
        Assert.Contains(Strings.NotEnoughSkinsToTransport, result.Records[0].Message);
    }

    [Fact]
    public void TransportSkins_ShouldSucceed_WhenEnoughResources()
    {
        // Arrange
        _mockViewModel.Setup(vm => vm.CurrentUserActivity).Returns(new UserInitiatedActivitySequence
        {
            Meta = new TimelapseActivityMeta { Name = "Transporting", Duration = 10, Elapsed = 0 }
        });

        // Act
        var result = _roleTrader.TransportSkins(_mockViewModel.Object, _exporter, 10);

        // Assert
        Assert.Equal(EventResultStatus.Success, result.Status);
        Assert.Contains("Transported 10 skins about 20 miles", result.Records[0].Message);
    }
}
