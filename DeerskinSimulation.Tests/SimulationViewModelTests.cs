using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using DeerskinSimulation.Commands;
using DeerskinSimulation.Models;
using DeerskinSimulation.Services;
using DeerskinSimulation.ViewModels;

namespace DeerskinSimulation.Tests
{
    public class SimulationViewModelTests
    {
        private readonly SimulationViewModel _viewModel;
        private readonly Mock<GameLoopService> _mockGameLoopService;
        private readonly Mock<StateContainer> _mockSession;

        public SimulationViewModelTests()
        {
            _mockSession = new Mock<StateContainer>();
            _mockGameLoopService = new Mock<GameLoopService>();

            _viewModel = new SimulationViewModel(_mockSession.Object, _mockGameLoopService.Object);
        }

        [Fact]
        public void Constructor_InitializesCommands()
        {
            Assert.NotNull(_viewModel.ConfirmSellCmd);
            Assert.NotNull(_viewModel.ConfirmHuntCmd);
            Assert.NotNull(_viewModel.ConfirmTransportCmd);
            Assert.NotNull(_viewModel.ConfirmExportCmd);
        }

        [Fact]
        public void Constructor_InitializesInstances()
        {
            Assert.NotNull(_viewModel.HunterInstance);
            Assert.NotNull(_viewModel.TraderInstance);
            Assert.NotNull(_viewModel.ExporterInstance);
        }

        [Fact]
        public async Task UpdateUserActivityDay_InvokesStartOnFirstDay()
        {
            // Arrange
            var mockActivity = new UserInitiatedActivitySequence
            {
                Meta = new TimelapseActivityMeta { Duration = 10 },
                Start = new Func<Task>(() => Task.CompletedTask),
                InProcess = new Func<Task>(() => Task.CompletedTask),
                Finish = new Func<Task>(() => Task.CompletedTask)
            };
            _viewModel.CurrentUserActivity = mockActivity;

            // Act
            await _viewModel.UpdateUserActivityDay();

            // Assert
            Assert.Equal(1, mockActivity.Meta.Elapsed);
        }

        [Fact]
        public async Task UpdateUserActivityDay_InvokesFinishOnLastDay()
        {
            // Arrange
            var mockActivity = new UserInitiatedActivitySequence
            {
                Meta = new TimelapseActivityMeta { Duration = 1, Elapsed = 0 },
                Start = new Func<Task>(() => Task.CompletedTask),
                InProcess = new Func<Task>(() => Task.CompletedTask),
                Finish = new Func<Task>(() => Task.CompletedTask)
            };
            _viewModel.CurrentUserActivity = mockActivity;

            // Act
            await _viewModel.UpdateUserActivityDay();

            // Assert
            Assert.Null(_viewModel.CurrentUserActivity);
        }

        //[Fact]
        //public async Task HandleNotification_AddsMessageAndInvokesStateChanged()
        //{
        //    // Arrange
        //    var mockEvent = new EventResult(new EventRecord("Test Message", "black"));
        //    var stateChangedInvoked = false;

        //    _viewModel.StateChanged += () =>
        //    {
        //        stateChangedInvoked = true;
        //        return Task.CompletedTask;
        //    };

        //    // Act
        //    _viewModel.HunterInstance.RaiseNotification("Test Message", "black");

        //    // Assert
        //    Assert.Contains(mockEvent, _viewModel.Messages);
        //    Assert.True(stateChangedInvoked);
        //}
    }
}
