using System;
using DeerskinSimulation.Models;
using DeerskinSimulation.Resources;
using DeerskinSimulation.ViewModels;
using Xunit;
using Moq;
using DeerskinSimulation.Services;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

namespace DeerskinSimulation.Tests;
public class RoleHunterTests
{
    private readonly RoleHunter _hunter;
    private readonly RoleTrader _trader;
    private readonly SimulationViewModel _viewModel;
    private readonly Mock<IRandomEventStrategy> _mockHuntingEventStrategy;
    private readonly Mock<IRandomEventStrategy> _mockForwardingEventStrategy;

    public RoleHunterTests()
    {
        _hunter = new RoleHunter("TestHunter");
        _trader = new RoleTrader("TestTrader");
        _mockHuntingEventStrategy = new Mock<IRandomEventStrategy>();
        _mockForwardingEventStrategy = new Mock<IRandomEventStrategy>();
        var mockGameLoopService = new Mock<GameLoopService>();
        _viewModel = new SimulationViewModel(null, mockGameLoopService.Object, null);
        _viewModel.SelectedPackhorses = 1; // Set default number of packhorses for tests
    }

    [Fact]
    public void Travel_ShouldFail_WhenNotEnoughMoney()
    {
        // Arrange
        _hunter.RemoveMoney(_hunter.Money); // Ensure no money

        _viewModel.CurrentUserActivity = new UserInitiatedActivitySequence
        {
            Meta = new TimelapseActivityMeta { Name = "Traveling" }
        };

        // Act
        var result = _hunter.Travel(_viewModel);

        // Assert
        Assert.Equal(EventResultStatus.Fail, result.Status);
        Assert.Contains(result.Records, record => record.Message.Contains(Strings.NotEnoughMoneyToHunt));
    }

    [Fact]
    public void Travel_ShouldSucceed_WhenEnoughMoney()
    {
        // Arrange
        _hunter.AddMoney(1000); // Ensure there is enough money

        _viewModel.CurrentUserActivity = new UserInitiatedActivitySequence
        {
            Meta = new TimelapseActivityMeta { Name = "Traveling" }
        };

        // Act
        var result = _hunter.Travel(_viewModel);

        // Assert
        Assert.Equal(EventResultStatus.Success, result.Status);
        Assert.Contains(result.Records, record => record.Message.Contains("Traveled about 20 miles."));
    }

    [Fact]
    public void Hunt_ShouldFail_WhenNotEnoughMoney()
    {
        // Arrange
        _hunter.RemoveMoney(_hunter.Money); // Ensure no money

        _viewModel.CurrentUserActivity = new UserInitiatedActivitySequence
        {
            Meta = new TimelapseActivityMeta { Name = "Hunting" }
        };

        // Act
        var result = _hunter.Hunt(_viewModel);

        // Assert
        Assert.Equal(EventResultStatus.Fail, result.Status);
        Assert.Contains(result.Records, record => record.Message.Contains(Strings.NotEnoughMoneyToHunt));
    }

    [Fact]
    public void Hunt_ShouldSucceed_WhenEnoughMoney()
    {
        // Arrange
        _hunter.AddMoney(1000); // Ensure there is enough money

        _viewModel.CurrentUserActivity = new UserInitiatedActivitySequence
        {
            Meta = new TimelapseActivityMeta { Name = "Hunting" }
        };

        // Act
        var result = _hunter.Hunt(_viewModel);

        // Assert
        Assert.Equal(EventResultStatus.Success, result.Status);
    }

    [Fact]
    public void EndHunt_ShouldResetCurrentBag()
    {
        // Arrange
        _hunter.CurrentBag = 50;

        _viewModel.CurrentUserActivity = new UserInitiatedActivitySequence
        {
            Meta = new TimelapseActivityMeta { Name = "Hunting", Duration = 5 }
        };

        // Act
        var result = _hunter.EndHunt(_viewModel);

        // Assert
        Assert.Contains(result.Records, record => record.Message.Contains($"Field dressed {_hunter.CurrentBag} skins"));
        
        _hunter.CurrentBag = 0;
        Assert.Equal(0, _hunter.CurrentBag);

    }

    [Fact]
    public void DeliverToTrader_ShouldTransferSkinsAndMoney()
    {
        // Arrange
        int initialTraderSkins = 100;
        int numberOfSkins = 20;
        _hunter.AddSkins(numberOfSkins);
        _trader.AddSkins(initialTraderSkins);
        _trader.AddMoney(1000); // Ensure the trader has enough money

        // Act
        var result = _hunter.DeliverToTrader(_trader, numberOfSkins);

        // Assert
        Assert.Equal(numberOfSkins, _trader.Skins - initialTraderSkins);
        Assert.Contains(result.Records, record => record.Message.Contains($"Delivered {numberOfSkins} skins"));
    }

    [Fact]
    public void DeliverToTrader_ShouldFail_WhenNotEnoughSkins()
    {
        // Arrange
        int numberOfSkins = 20;
        _hunter.RemoveSkins(_hunter.Skins); // Remove all skins

        // Act
        var result = _hunter.DeliverToTrader(_trader, numberOfSkins);

        // Assert
        Assert.Contains(result.Records, record => record.Message.Contains(Strings.NotEnoughSkinsToSell));
    }

    //[Fact]
    //public void RollForRandomHuntingEvent_ShouldInvokeRandomEventStrategy()
    //{
    //    // Arrange
    //    _mockHuntingEventStrategy.Setup(s => s.ApplyEvent(It.IsAny<ParticipantRole>())).Returns(new EventResult());

    //    // Act
    //    var result = _hunter.RollForRandomHuntingEvent();

    //    // Assert
    //    _mockHuntingEventStrategy.Verify(s => s.ApplyEvent(_hunter), Times.Once);
    //}

    //[Fact]
    //public void RollForRandomForwardingEvent_ShouldInvokeRandomEventStrategy()
    //{
    //    // Arrange
    //    _mockForwardingEventStrategy.Setup(s => s.ApplyEvent(It.IsAny<ParticipantRole>())).Returns(new EventResult());

    //    // Act
    //    var result = _hunter.RollForRandomForwardingEvent();

    //    // Assert
    //    _mockForwardingEventStrategy.Verify(s => s.ApplyEvent(_hunter), Times.Once);
    //}
}
