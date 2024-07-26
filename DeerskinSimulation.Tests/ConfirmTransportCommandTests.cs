using System.Threading.Tasks;
using DeerskinSimulation.Commands;
using DeerskinSimulation.Models;
using DeerskinSimulation.Services;
using DeerskinSimulation.ViewModels;
using Moq;
using Xunit;

namespace DeerskinSimulation.Tests
{
    public class ConfirmTransportCommandTests
    {
        private readonly Mock<SimulationViewModel> _mockViewModel;
        private readonly Mock<GameLoopService> _mockGameLoopService;
        private readonly ConfirmTransportCommand _confirmTransportCommand;

        public ConfirmTransportCommandTests()
        {
            _mockViewModel = new Mock<SimulationViewModel>(null, null);
            _mockGameLoopService = new Mock<GameLoopService>();
            _confirmTransportCommand = new ConfirmTransportCommand(_mockViewModel.Object, _mockGameLoopService.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldStartActivity_WhenNumberOfSkinsIsGreaterThanZero()
        {
            // Arrange
            var transportOptions = new TransportOptionsViewModel { NumberOfSkins = 100 };

            // Act
            await _confirmTransportCommand.ExecuteAsync(transportOptions);

            // Assert
            _mockGameLoopService.Verify(s => s.StartActivity(It.IsAny<TimelapseActivityMeta>()), Times.Once);
            Assert.NotNull(_mockViewModel.Object.CurrentUserActivity);
            Assert.Equal("Transporting", _mockViewModel.Object.CurrentUserActivity.Meta.Name);
            Assert.Equal(10, _mockViewModel.Object.CurrentUserActivity.Meta.Duration);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldNotStartActivity_WhenNumberOfSkinsIsZeroOrLess()
        {
            // Arrange
            var transportOptions = new TransportOptionsViewModel { NumberOfSkins = 0 };

            // Act
            await _confirmTransportCommand.ExecuteAsync(transportOptions);

            // Assert
            _mockGameLoopService.Verify(s => s.StartActivity(It.IsAny<TimelapseActivityMeta>()), Times.Never);
            Assert.Null(_mockViewModel.Object.CurrentUserActivity);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldInvokeTransportToExporterOnStart()
        {
            // Arrange
            var transportOptions = new TransportOptionsViewModel { NumberOfSkins = 100 };
            _mockViewModel.Setup(vm => vm.TransportToExporter(It.IsAny<int>())).Returns(Task.CompletedTask);

            // Act
            await _confirmTransportCommand.ExecuteAsync(transportOptions);

            // Assert
            await _mockViewModel.Object.CurrentUserActivity.Start();
            _mockViewModel.Verify(vm => vm.TransportToExporter(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldSetCurrentUserActivityToNullOnFinish()
        {
            // Arrange
            var transportOptions = new TransportOptionsViewModel { NumberOfSkins = 100 };

            // Act
            await _confirmTransportCommand.ExecuteAsync(transportOptions);

            // Assert
            await _mockViewModel.Object.CurrentUserActivity.Finish();
            Assert.Null(_mockViewModel.Object.CurrentUserActivity);
        }
    }
}
