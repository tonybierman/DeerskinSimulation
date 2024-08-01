using System;
using DeerskinSimulation.Models;
using DeerskinSimulation.Resources;
using DeerskinSimulation.ViewModels;
using Xunit;
using Moq;
using DeerskinSimulation.Services;

namespace DeerskinSimulation.Tests;

public class RoleTraderTests
{
    private readonly RoleTrader _trader;
    private readonly RoleExporter _exporter;
    private readonly SimulationViewModel _viewModel;
    private readonly Mock<IRandomEventStrategy> _mockEventStrategy;

    public RoleTraderTests()
    {
        _trader = new RoleTrader("TestTrader");
        _exporter = new RoleExporter("TestExporter");
        _mockEventStrategy = new Mock<IRandomEventStrategy>();
        var mockGameLoopService = new Mock<GameLoopService>();
        _viewModel = new SimulationViewModel(null, mockGameLoopService.Object, null);
    }

    [Fact]
    public void DeliverToExporter_ShouldTransferSkinsAndMoney()
    {
        // Arrange
        int initialTraderSkins = 100;
        int initialExporterSkins = 50;
        int numberOfSkins = 20;
        _trader.AddSkins(initialTraderSkins);
        _exporter.AddSkins(initialExporterSkins);

        // Act
        var result = _trader.DeliverToExporter(_viewModel, _exporter, numberOfSkins);

        // Assert
        Assert.Equal(initialTraderSkins - numberOfSkins, _trader.Skins);
        Assert.Equal(initialExporterSkins + numberOfSkins, _exporter.Skins);
        Assert.Contains(result.Records, record => record.Message.Contains($"Delivered {numberOfSkins} skins to exporter."));
    }

    [Fact]
    public void TransportSkins_ShouldFail_WhenNotEnoughMoney()
    {
        // Arrange
        int numberOfSkins = 10;
        _trader.RemoveMoney(_trader.Money); // Remove all money

        _viewModel.CurrentUserActivity = new UserInitiatedActivitySequence
        {
            Meta = new TimelapseActivityMeta { Name = "Transporting" }
        };

        // Act
        var result = _trader.TransportSkins(_viewModel, _exporter, numberOfSkins);

        // Assert
        Assert.Equal(EventResultStatus.Fail, result.Status);
        Assert.Contains(result.Records, record => record.Message.Contains(Strings.NotEnoughMoneyTravel));
    }

    [Fact]
    public void TransportSkins_ShouldFail_WhenNotEnoughSkins()
    {
        // Arrange
        int numberOfSkins = 10;
        _trader.RemoveSkins(_trader.Skins); // Remove all skins
        _trader.AddMoney(1000); // Ensure there is enough money

        _viewModel.CurrentUserActivity = new UserInitiatedActivitySequence
        {
            Meta = new TimelapseActivityMeta { Name = "Transporting" }
        };

        // Act
        var result = _trader.TransportSkins(_viewModel, _exporter, numberOfSkins);

        // Assert
        Assert.Equal(EventResultStatus.Fail, result.Status);
        Assert.Contains(result.Records, record => record.Message.Contains(Strings.NotEnoughSkinsToTransport));
    }

    [Fact]
    public void TransportSkins_ShouldSucceed_WhenEnoughMoneyAndSkins()
    {
        // Arrange
        int numberOfSkins = 10;
        _trader.AddSkins(numberOfSkins);
        _trader.AddMoney(1000); // Ensure there is enough money

        _viewModel.CurrentUserActivity = new UserInitiatedActivitySequence
        {
            Meta = new TimelapseActivityMeta { Name = "Transporting" }
        };

        // Act
        var result = _trader.TransportSkins(_viewModel, _exporter, numberOfSkins);

        // Assert
        Assert.Equal(EventResultStatus.Success, result.Status);
        Assert.Contains(result.Records, record => record.Message.Contains($"Transported {numberOfSkins} about 20 miles."));
    }

    //[Fact]
    //public void RollForRandomTransportingEvent_ShouldInvokeRandomEventStrategy()
    //{
    //    // Arrange
    //    _mockEventStrategy.Setup(s => s.ApplyEvent(It.IsAny<ParticipantRole>())).Returns(new EventResult());

    //    // Act
    //    var result = _trader.RollForRandomTransportingEvent();

    //    // Assert
    //    _mockEventStrategy.Verify(s => s.ApplyEvent(_trader), Times.Once);
    //}
}
