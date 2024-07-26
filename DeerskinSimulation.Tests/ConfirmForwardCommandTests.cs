using System.Threading.Tasks;
using DeerskinSimulation.Commands;
using DeerskinSimulation.Models;
using DeerskinSimulation.Services;
using DeerskinSimulation.ViewModels;
using Moq;
using Xunit;

namespace DeerskinSimulation.Tests
{
    public class ConfirmForwardCommandTests
    {
        private readonly Mock<SimulationViewModel> _mockViewModel;
        private readonly Mock<GameLoopService> _mockGameLoopService;
        private readonly ConfirmForwardCommand _confirmForwardCommand;

        public ConfirmForwardCommandTests()
        {
            _mockViewModel = new Mock<SimulationViewModel>(null, null);
            _mockGameLoopService = new Mock<GameLoopService>();
            _confirmForwardCommand = new ConfirmForwardCommand(_mockViewModel.Object, _mockGameLoopService.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldStartActivity_WhenNumberOfSkinsIsGreaterThanZero()
        {
            // Arrange
            var forwardOptions = new ForwardOptionsViewModel { NumberOfSkins = 10 };

            // Act
            await _confirmForwardCommand.ExecuteAsync(forwardOptions);

            // Assert
            _mockGameLoopService.Verify(s => s.StartActivity(It.IsAny<TimelapseActivityMeta>()), Times.Once);
            Assert.NotNull(_mockViewModel.Object.CurrentUserActivity);
            Assert.Equal("Forwarding", _mockViewModel.Object.CurrentUserActivity.Meta.Name);
            Assert.Equal(10, _mockViewModel.Object.CurrentUserActivity.Meta.Duration);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldNotStartActivity_WhenNumberOfSkinsIsZeroOrLess()
        {
            // Arrange
            var forwardOptions = new ForwardOptionsViewModel { NumberOfSkins = 0 };

            // Act
            await _confirmForwardCommand.ExecuteAsync(forwardOptions);

            // Assert
            _mockGameLoopService.Verify(s => s.StartActivity(It.IsAny<TimelapseActivityMeta>()), Times.Never);
            Assert.Null(_mockViewModel.Object.CurrentUserActivity);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldInvokeRandomForwardingEventCheckOnInProcess()
        {
            // Arrange
            var forwardOptions = new ForwardOptionsViewModel { NumberOfSkins = 10 };
            _mockViewModel.Setup(vm => vm.RandomForwardingEventCheck()).Returns(Task.CompletedTask);

            // Act
            await _confirmForwardCommand.ExecuteAsync(forwardOptions);

            // Assert
            await _mockViewModel.Object.CurrentUserActivity.InProcess();
            _mockViewModel.Verify(vm => vm.RandomForwardingEventCheck(), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldInvokeForwardToTraderOnFinish()
        {
            // Arrange
            var forwardOptions = new ForwardOptionsViewModel { NumberOfSkins = 10 };
            _mockViewModel.Setup(vm => vm.ForwardToTrader(It.IsAny<int>())).Returns(Task.CompletedTask);

            // Act
            await _confirmForwardCommand.ExecuteAsync(forwardOptions);

            // Assert
            await _mockViewModel.Object.CurrentUserActivity.Finish();
            _mockViewModel.Verify(vm => vm.ForwardToTrader(It.IsAny<int>()), Times.Once);
            Assert.Null(_mockViewModel.Object.CurrentUserActivity);
        }
    }
}
