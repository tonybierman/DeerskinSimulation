using System.Threading.Tasks;
using DeerskinSimulation.Commands;
using DeerskinSimulation.Models;
using DeerskinSimulation.Services;
using DeerskinSimulation.ViewModels;
using Moq;
using Xunit;

namespace DeerskinSimulation.Tests
{
    public class ConfirmExportCommandTests
    {
        private readonly Mock<SimulationViewModel> _mockViewModel;
        private readonly Mock<GameLoopService> _mockGameLoopService;
        private readonly ConfirmExportCommand _confirmExportCommand;

        public ConfirmExportCommandTests()
        {
            _mockViewModel = new Mock<SimulationViewModel>(null, null);
            _mockGameLoopService = new Mock<GameLoopService>();
            _confirmExportCommand = new ConfirmExportCommand(_mockViewModel.Object, _mockGameLoopService.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldStartActivity_WhenNumberOfSkinsIsGreaterThanZero()
        {
            // Arrange
            var exportOptions = new ExportOptionsViewModel { NumberOfSkins = 10 };

            // Act
            await _confirmExportCommand.ExecuteAsync(exportOptions);

            // Assert
            _mockGameLoopService.Verify(s => s.StartActivity(It.IsAny<TimedActivityMeta>()), Times.Once);
            Assert.NotNull(_mockViewModel.Object.CurrentUserActivity);
            Assert.Equal("Exporting", _mockViewModel.Object.CurrentUserActivity.Meta.Name);
            Assert.Equal(10, _mockViewModel.Object.CurrentUserActivity.Meta.Duration);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldNotStartActivity_WhenNumberOfSkinsIsZeroOrLess()
        {
            // Arrange
            var exportOptions = new ExportOptionsViewModel { NumberOfSkins = 0 };

            // Act
            await _confirmExportCommand.ExecuteAsync(exportOptions);

            // Assert
            _mockGameLoopService.Verify(s => s.StartActivity(It.IsAny<TimedActivityMeta>()), Times.Never);
            Assert.Null(_mockViewModel.Object.CurrentUserActivity);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldInvokeExportOnStart()
        {
            // Arrange
            var exportOptions = new ExportOptionsViewModel { NumberOfSkins = 10 };
            _mockViewModel.Setup(vm => vm.Export(It.IsAny<int>())).Returns(Task.CompletedTask);

            // Act
            await _confirmExportCommand.ExecuteAsync(exportOptions);

            // Assert
            await _mockViewModel.Object.CurrentUserActivity.Start();
            _mockViewModel.Verify(vm => vm.Export(exportOptions.NumberOfSkins), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldSetCurrentUserActivityToNullOnFinish()
        {
            // Arrange
            var exportOptions = new ExportOptionsViewModel { NumberOfSkins = 10 };

            // Act
            await _confirmExportCommand.ExecuteAsync(exportOptions);

            // Assert
            await _mockViewModel.Object.CurrentUserActivity.Finish();
            Assert.Null(_mockViewModel.Object.CurrentUserActivity);
        }
    }
}
