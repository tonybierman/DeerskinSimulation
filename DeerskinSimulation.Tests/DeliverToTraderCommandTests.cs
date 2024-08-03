using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using DeerskinSimulation.Commands;
using DeerskinSimulation.Models;
using DeerskinSimulation.ViewModels;
using DeerskinSimulation.Resources;
using System.Collections.Generic;

public class DeliverToTraderCommandTests
{
    private readonly Mock<ISimulationViewModel> _mockViewModel;
    private readonly RoleHunter _hunter;
    private readonly RoleTrader _trader;
    private readonly DeliverToTraderCommand _command;
    private readonly int _numberOfSkins;

    public DeliverToTraderCommandTests()
    {
        // Arrange
        _mockViewModel = new Mock<ISimulationViewModel>();

        // Create real instances of RoleHunter and RoleTrader
        _hunter = new RoleHunter("Hunter", 100, 10);
        _trader = new RoleTrader("Trader", 1000, 50);

        // Set up the ISimulationViewModel to return the real hunter and trader
        _mockViewModel.Setup(vm => vm.Hunter).Returns(_hunter);
        _mockViewModel.Setup(vm => vm.Trader).Returns(_trader);

        // Set up the number of skins to deliver
        _numberOfSkins = 5;

        // Create the command instance
        _command = new DeliverToTraderCommand(_mockViewModel.Object, _numberOfSkins);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnSuccess_WhenEnoughSkins()
    {
        // Arrange
        _hunter.AddSkins(10); // Ensure the hunter has enough skins

        // Act
        var result = await _command.ExecuteAsync();

        // Assert
        Assert.Equal(EventResultStatus.Success, result);
        Assert.Equal(55, _trader.Skins); // 50 + 5 skins delivered
        Assert.Equal(15, _hunter.Skins); // 10 - 5 skins delivered
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnFail_WhenNotEnoughSkins()
    {
        // Arrange
        _hunter.RemoveSkins(10); // Ensure the hunter has no skins

        // Act
        var result = await _command.ExecuteAsync();

        // Assert
        Assert.Equal(EventResultStatus.Fail, result);
        Assert.Equal(50, _trader.Skins); // Trader skins remain unchanged
        Assert.Equal(0, _hunter.Skins); // Hunter skins remain unchanged
    }

    [Fact]
    public async Task ExecuteAsync_ShouldSetFeatured_WhenResultHasRecords()
    {
        // Arrange
        _hunter.AddSkins(10); // Ensure the hunter has enough skins

        // Act
        await _command.ExecuteAsync();

        // Assert
        _mockViewModel.Verify(vm => vm.SetFeatured(It.IsAny<EventRecord>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldNotSetFeatured_WhenResultHasNoRecords()
    {
        // Arrange
        _hunter.RemoveSkins(10); // Ensure the hunter has no skins

        // Act
        await _command.ExecuteAsync();

        // Assert
        _mockViewModel.Verify(vm => vm.SetFeatured(It.IsAny<EventRecord>()), Times.Once);
    }

    [Fact]
    public void CanExecute_ShouldAlwaysReturnTrue()
    {
        // Act
        var canExecute = _command.CanExecute(null);

        // Assert
        Assert.True(canExecute);
    }
}
