using System.Threading.Tasks;
using DeerskinSimulation.Commands;
using DeerskinSimulation.Models;
using DeerskinSimulation.Services;
using DeerskinSimulation.ViewModels;
using Moq;
using Xunit;

namespace DeerskinSimulation.Tests
{
    public class ConfirmHuntCommandTests
    {
        private readonly Mock<SimulationViewModel> _mockViewModel;
        private readonly Mock<GameLoopService> _mockGameLoopService;
        private readonly ConfirmHuntCommand _confirmHuntCommand;

        public ConfirmHuntCommandTests()
        {
            _mockViewModel = new Mock<SimulationViewModel>(null, null);
            _mockGameLoopService = new Mock<GameLoopService>();
            _confirmHuntCommand = new ConfirmHuntCommand(_mockViewModel.Object, _mockGameLoopService.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldStartActivity_WhenCalled()
        {
            // Arrange
            var huntOptions = new HuntOptionsViewModel { SelectedPackhorses = 3 };

            // Act
            await _confirmHuntCommand.ExecuteAsync(huntOptions);

            // Assert
            _mockGameLoopService.Verify(s => s.StartActivity(It.IsAny<TimelapseActivityMeta>()), Times.Once);
            Assert.NotNull(_mockViewModel.Object.CurrentUserActivity);
            Assert.Equal("Hunting", _mockViewModel.Object.CurrentUserActivity.Meta.Name);
            Assert.Equal(10, _mockViewModel.Object.CurrentUserActivity.Meta.Duration);
        }

        //[Fact]
        //public async Task ExecuteAsync_ShouldInvokeHuntOnStart()
        //{
        //    // Arrange
        //    var huntOptions = new HuntOptionsViewModel { SelectedPackhorses = 3 };
        //    _mockViewModel.Setup(vm => vm.Hunt()).Returns(Task.CompletedTask);

        //    // Act
        //    await _confirmHuntCommand.ExecuteAsync(huntOptions);

        //    // Assert
        //    await _mockViewModel.Object.CurrentUserActivity.Start();
        //    _mockViewModel.Verify(vm => vm.Hunt(), Times.Once);
        //}

        [Fact]
        public async Task ExecuteAsync_ShouldInvokeRandomHuntingEventCheckOnInProcess()
        {
            // Arrange
            var huntOptions = new HuntOptionsViewModel { SelectedPackhorses = 3 };
            _mockViewModel.Setup(vm => vm.RandomHuntingEventCheck()).Returns(Task.CompletedTask);

            // Act
            await _confirmHuntCommand.ExecuteAsync(huntOptions);

            // Assert
            await _mockViewModel.Object.CurrentUserActivity.InProcess();
            _mockViewModel.Verify(vm => vm.RandomHuntingEventCheck(), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldSetCurrentUserActivityToNullOnFinish()
        {
            // Arrange
            var huntOptions = new HuntOptionsViewModel { SelectedPackhorses = 3 };

            // Act
            await _confirmHuntCommand.ExecuteAsync(huntOptions);

            // Assert
            await _mockViewModel.Object.CurrentUserActivity.Finish();
            Assert.Null(_mockViewModel.Object.CurrentUserActivity);
        }
    }
}
