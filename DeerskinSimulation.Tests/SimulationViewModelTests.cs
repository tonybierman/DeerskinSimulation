using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DeerskinSimulation.Commands;
using DeerskinSimulation.Models;
using DeerskinSimulation.Services;
using DeerskinSimulation.ViewModels;
using Moq;
using Xunit;

namespace DeerskinSimulation.Tests;
public class SimulationViewModelTests
{
    private readonly Mock<StateContainer> _mockSession;
    private readonly Mock<GameLoopService> _mockGameLoopService;
    private readonly HttpClient _httpClient;
    private readonly SimulationViewModel _viewModel;

    public SimulationViewModelTests()
    {
        _mockSession = new Mock<StateContainer>();
        _mockGameLoopService = new Mock<GameLoopService>();
        _httpClient = new HttpClient();
        _viewModel = new SimulationViewModel(_mockSession.Object, _mockGameLoopService.Object, _httpClient);
    }

    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        Assert.NotNull(_viewModel.Hunter);
        Assert.NotNull(_viewModel.Trader);
        Assert.NotNull(_viewModel.Exporter);
        Assert.NotNull(_viewModel.ConfirmSellCmd);
        Assert.NotNull(_viewModel.ConfirmHuntCmd);
        Assert.NotNull(_viewModel.ConfirmTransportCmd);
        Assert.NotNull(_viewModel.ConfirmExportCmd);
        Assert.Equal(1, _viewModel.SelectedPackhorses);
    }

    [Fact]
    public void AddMessage_ShouldAddMessageToList()
    {
        var message = new EventResult();
        _viewModel.AddMessage(message);

        Assert.Single(_viewModel.Messages);
        Assert.Contains(message, _viewModel.Messages);
    }

    [Fact]
    public void AddMessage_ShouldInvokeMessageAddedEvent()
    {
        var message = new EventResult();
        bool eventInvoked = false;

        _viewModel.MessageAdded += (msg) => eventInvoked = true;
        _viewModel.AddMessage(message);

        Assert.True(eventInvoked);
    }

    [Fact]
    public void ClearMessages_ShouldClearMessagesList()
    {
        _viewModel.AddMessage(new EventResult());
        Assert.NotEmpty(_viewModel.Messages);

        _viewModel.ClearMessages();
        Assert.Empty(_viewModel.Messages);
    }

    [Fact]
    public void ClearMessages_ShouldInvokeMessagesClearedEvent()
    {
        bool eventInvoked = false;
        _viewModel.MessagesCleared += () => eventInvoked = true;

        _viewModel.ClearMessages();
        Assert.True(eventInvoked);
    }

    [Fact]
    public async Task UpdateUserActivityDay_ShouldInvokeStartAndFinishCorrectly()
    {
        bool startInvoked = false;
        bool finishInvoked = false;

        var activity = new UserInitiatedActivitySequence
        {
            Meta = new TimelapseActivityMeta { Duration = 2 },
            Start = async () => startInvoked = true,
            Finish = async () => finishInvoked = true
        };

        _viewModel.CurrentUserActivity = activity;

        await _viewModel.UpdateUserActivityDay(); // First day
        Assert.True(startInvoked);
        Assert.False(finishInvoked);

        await _viewModel.UpdateUserActivityDay(); // Last day
        Assert.True(finishInvoked);
    }
}
