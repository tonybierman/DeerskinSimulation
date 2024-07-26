using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeerskinSimulation.Models;
using DeerskinSimulation.Services;
using DeerskinSimulation.ViewModels;
using Xunit;
using Moq;

namespace DeerskinSimulation.Tests
{
    public class SimulationViewModelTests
    {
        private readonly Mock<GameLoopService> _mockGameLoopService;
        private readonly StateContainer _stateContainer;
        private readonly SimulationViewModel _viewModel;

        public SimulationViewModelTests()
        {
            _mockGameLoopService = new Mock<GameLoopService>();
            _stateContainer = new StateContainer();
            _viewModel = new SimulationViewModel(_stateContainer, _mockGameLoopService.Object);
            _viewModel.StateChanged += async () => await Task.CompletedTask; // Ensure StateChanged is subscribed
        }

        [Fact]
        public async Task Hunt_ShouldAddEventResultToMessages()
        {
            // Arrange
            var initialMessagesCount = _viewModel.Messages.Count;

            // Act
            await _viewModel.Hunt();

            // Assert
            Assert.NotEqual(initialMessagesCount, _viewModel.Messages.Count);
        }

        [Fact]
        public async Task ForwardToTrader_ShouldAddEventResultToMessages()
        {
            // Arrange
            var initialMessagesCount = _viewModel.Messages.Count;
            _viewModel.HunterInstance.AddSkins(10); // Ensure hunter has skins to sell

            // Act
            await _viewModel.ForwardToTrader(10);

            // Assert
            Assert.NotEqual(initialMessagesCount, _viewModel.Messages.Count);
        }

        [Fact]
        public async Task TransportToExporter_ShouldAddEventResultToMessages()
        {
            // Arrange
            var initialMessagesCount = _viewModel.Messages.Count;
            _viewModel.TraderInstance.AddSkins(10); // Ensure trader has skins to transport

            // Act
            await _viewModel.TransportToExporter(10);

            // Assert
            Assert.NotEqual(initialMessagesCount, _viewModel.Messages.Count);
        }

        [Fact]
        public async Task Export_ShouldAddEventResultToMessages()
        {
            // Arrange
            var initialMessagesCount = _viewModel.Messages.Count;
            _viewModel.ExporterInstance.AddSkins(10); // Ensure exporter has skins to export

            // Act
            await _viewModel.Export(10);

            // Assert
            Assert.NotEqual(initialMessagesCount, _viewModel.Messages.Count);
        }

        [Fact]
        public async Task UpdateDay_ShouldInvokeStartAndFinishActions()
        {
            // Arrange
            var startInvoked = false;
            var finishInvoked = false;
            var userActivity = new UserActivity
            {
                Meta = new TimedActivityMeta { Duration = 1 },
                Start = () => { startInvoked = true; return Task.CompletedTask; },
                Finish = () => { finishInvoked = true; return Task.CompletedTask; }
            };
            _viewModel.CurrentUserActivity = userActivity;

            // Act
            _viewModel.UpdateDay();
            await Task.Delay(50); // Wait for the async operation to complete

            // Assert
            Assert.True(startInvoked);
            Assert.True(finishInvoked);
        }
    }
}
