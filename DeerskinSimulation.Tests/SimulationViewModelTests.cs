using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeerskinSimulation.Models;
using DeerskinSimulation.ViewModels;
using Xunit;

namespace DeerskinSimulation.Tests
{
    public class SimulationViewModelTests
    {
        [Fact]
        public void SimulationViewModel_InitializesCorrectly()
        {
            // Arrange
            var session = new StateContainer();
            var viewModel = new SimulationViewModel(session);

            // Act & Assert
            Assert.NotNull(viewModel);
            Assert.NotNull(viewModel.HunterInstance);
            Assert.NotNull(viewModel.TraderInstance);
            Assert.NotNull(viewModel.ExporterInstance);
            Assert.NotNull(viewModel.Messages);
            Assert.Equal(1, viewModel.SelectedPackhorses);
        }

        [Fact]
        public async Task Hunt_ShouldAddEventResultToMessages()
        {
            // Arrange
            var session = new StateContainer();
            var viewModel = new SimulationViewModel(session);
            var stateChangedInvoked = false;
            viewModel.StateChanged += () => { stateChangedInvoked = true; return Task.CompletedTask; };

            // Act
            await viewModel.Hunt();

            // Assert
            Assert.True(viewModel.Messages.Count > 0);
            Assert.True(stateChangedInvoked);
        }

        [Fact]
        public async Task SellToTrader_ShouldAddEventResultToMessages()
        {
            // Arrange
            var session = new StateContainer();
            var viewModel = new SimulationViewModel(session);
            var stateChangedInvoked = false;
            viewModel.StateChanged += () => { stateChangedInvoked = true; return Task.CompletedTask; };

            // Act
            await viewModel.SellToTrader(10);

            // Assert
            Assert.True(viewModel.Messages.Count > 0);
            Assert.True(stateChangedInvoked);
        }

        [Fact]
        public async Task TransportToExporter_ShouldAddEventResultToMessages()
        {
            // Arrange
            var session = new StateContainer();
            var viewModel = new SimulationViewModel(session);
            var stateChangedInvoked = false;
            viewModel.StateChanged += () => { stateChangedInvoked = true; return Task.CompletedTask; };

            // Act
            await viewModel.TransportToExporter(10);

            // Assert
            Assert.True(viewModel.Messages.Count > 0);
            Assert.True(stateChangedInvoked);
        }

        [Fact]
        public async Task Export_ShouldAddEventResultToMessages()
        {
            // Arrange
            var session = new StateContainer();
            var viewModel = new SimulationViewModel(session);
            var stateChangedInvoked = false;
            viewModel.StateChanged += () => { stateChangedInvoked = true; return Task.CompletedTask; };

            // Act
            await viewModel.Export(10);

            // Assert
            Assert.True(viewModel.Messages.Count > 0);
            Assert.True(stateChangedInvoked);
        }

        [Fact]
        public void DebugMode_ShouldReflectStateContainerValue()
        {
            // Arrange
            var session = new StateContainer { Debug = true };
            var viewModel = new SimulationViewModel(session);

            // Act & Assert
            Assert.True(viewModel.Debug);

            // Arrange with Debug mode off
            session = new StateContainer { Debug = false };
            viewModel = new SimulationViewModel(session);

            // Act & Assert
            Assert.False(viewModel.Debug);
        }

        [Fact]
        public async Task HuntRandomEventCheck_ShouldAddDebugMessageInDebugMode()
        {
            // Arrange
            var session = new StateContainer { Debug = true };
            var viewModel = new SimulationViewModel(session);
            var stateChangedInvoked = false;
            viewModel.StateChanged += () => { stateChangedInvoked = true; return Task.CompletedTask; };

            // Act
            await viewModel.HuntRandomEventCheck();

            // Assert
            Assert.Contains(viewModel.Messages, msg => msg.Records.Exists(record => record.Message == "Random event check"));
            Assert.True(stateChangedInvoked);
        }

        //[Fact]
        //public async Task HandleNotification_ShouldAddEventResultToMessages()
        //{
        //    // Arrange
        //    var session = new StateContainer();
        //    var viewModel = new SimulationViewModel(session);
        //    var stateChangedInvoked = false;
        //    viewModel.StateChanged += () => { stateChangedInvoked = true; return Task.CompletedTask; };
        //    var eventResult = new EventResult(new EventRecord("Test Event"));

        //    // Act
        //    viewModel.HandleNotification(this, eventResult);

        //    // Assert
        //    Assert.Contains(viewModel.Messages, msg => msg.Records.Exists(record => record.Message == "Test Event"));
        //    Assert.True(stateChangedInvoked);
        //}
    }
}

