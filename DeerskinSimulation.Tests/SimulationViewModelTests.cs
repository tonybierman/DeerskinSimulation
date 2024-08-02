using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeerskinSimulation.Commands;
using DeerskinSimulation.Models;
using DeerskinSimulation.Services;
using DeerskinSimulation.ViewModels;

namespace DeerskinSimulation.Tests
{
    public class SimulationViewModelTests
    {
        private readonly Mock<IGameLoopService> _mockGameLoopService;
        private readonly Mock<ICommandFactory> _mockCommandFactory;
        private readonly Mock<IStateContainer> _mockSession;
        private readonly SimulationViewModel _viewModel;

        public SimulationViewModelTests()
        {
            _mockGameLoopService = new Mock<IGameLoopService>();
            _mockCommandFactory = new Mock<ICommandFactory>();
            _mockSession = new Mock<IStateContainer>();

            _viewModel = new SimulationViewModel(
                _mockGameLoopService.Object,
                _mockCommandFactory.Object,
                _mockSession.Object);
        }

        [Fact]
        public void Constructor_ShouldInitializeComponents()
        {
            // Assert
            Assert.NotNull(_viewModel.Hunter);
            Assert.NotNull(_viewModel.Trader);
            Assert.NotNull(_viewModel.Exporter);
            Assert.NotNull(_viewModel.EnsureSellCmd);
            Assert.NotNull(_viewModel.EnsureHuntCmd);
            Assert.NotNull(_viewModel.EnsureTransportCmd);
            Assert.NotNull(_viewModel.EnsureExportCmd);
        }

        [Fact]
        public void AddMessage_ShouldAddMessageAndInvokeEvent()
        {
            // Arrange
            var eventTriggered = false;
            var eventResult = new EventResult(new EventRecord("Test message"));

            _viewModel.MessageAdded += (message) => eventTriggered = true;

            // Act
            _viewModel.AddMessage(eventResult);

            // Assert
            Assert.Contains(eventResult, _viewModel.Messages);
            Assert.True(eventTriggered);
        }

        [Fact]
        public void ClearMessages_ShouldClearAllMessagesAndInvokeEvent()
        {
            // Arrange
            var eventTriggered = false;
            _viewModel.AddMessage(new EventResult(new EventRecord("Test message")));
            _viewModel.MessagesCleared += () => eventTriggered = true;

            // Act
            _viewModel.ClearMessages();

            // Assert
            Assert.Empty(_viewModel.Messages);
            Assert.True(eventTriggered);
        }

        [Fact]
        public void SetFeatured_ShouldSetFeaturedStory()
        {
            // Arrange
            var eventRecord = new EventRecord("Featured Event");

            // Act
            _viewModel.SetFeatured(eventRecord);

            // Assert
            Assert.NotNull(_viewModel.Featured);
            Assert.Equal("Featured Event", _viewModel.Featured?.Record?.Message);
        }

        [Fact]
        public async Task UpdateUserActivityDay_ShouldInvokeStartAndFinish()
        {
            // Arrange
            var startInvoked = false;
            var finishInvoked = false;
            var activity = new UserInitiatedActivitySequence
            {
                Meta = new TimelapseActivityMeta { Duration = 1, Elapsed = 0 },
                Start = () =>
                {
                    startInvoked = true;
                    return Task.CompletedTask;
                },
                Finish = () =>
                {
                    finishInvoked = true;
                    return Task.CompletedTask;
                }
            };

            _viewModel.CurrentUserActivity = activity;

            // Act
            await _viewModel.UpdateUserActivityDay();

            // Assert
            Assert.True(startInvoked);
            Assert.True(finishInvoked);
            Assert.Null(_viewModel.CurrentUserActivity);
        }

        [Fact]
        public async Task UpdateUserActivityDay_ShouldInvokeInProcess()
        {
            // Arrange
            var inProcessInvoked = false;
            var activity = new UserInitiatedActivitySequence
            {
                Meta = new TimelapseActivityMeta { Duration = 2, Elapsed = 0 },
                InProcess = () =>
                {
                    inProcessInvoked = true;
                    return Task.CompletedTask;
                }
            };

            _viewModel.CurrentUserActivity = activity;

            // Act
            await _viewModel.UpdateUserActivityDay(); // This should invoke Start and InProcess
            await _viewModel.UpdateUserActivityDay(); // This should invoke InProcess and Finish

            // Assert
            Assert.True(inProcessInvoked);
        }
    }
}
