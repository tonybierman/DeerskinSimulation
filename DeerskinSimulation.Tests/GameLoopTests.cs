using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeerskinSimulation.Models;
using DeerskinSimulation.Services;
using System;
using System.Threading.Tasks;
using Xunit;



namespace DeerskinSimulation.Tests
{
    public class GameLoopServiceTests
    {
        [Fact]
        public void GameLoopService_InitializesCorrectly()
        {
            // Arrange & Act
            var gameLoopService = new GameLoopService();

            // Assert
            Assert.NotNull(gameLoopService);
            Assert.Null(gameLoopService.CurrentActivityMeta); // Corrected to check for null
            Assert.InRange(gameLoopService.FPS, 0, double.MaxValue); // Corrected to check for a valid FPS range
        }

        [Fact]
        public async Task GameLoopService_TicksCorrectly()
        {
            // Arrange
            var gameLoopService = new GameLoopService();
            var tickCount = 0;
            gameLoopService.OnGameTick += () => tickCount++;

            // Act
            await Task.Delay(100); // Allow some time for ticks to occur

            // Assert
            Assert.True(tickCount > 0);
        }

        [Fact]
        public async Task GameLoopService_CalculatesFPSCorrectly()
        {
            // Arrange
            var gameLoopService = new GameLoopService();

            // Act
            await Task.Delay(1000); // Allow some time for FPS to be calculated

            // Assert
            Assert.True(gameLoopService.FPS > 0);
        }

        [Fact]
        public void GameLoopService_StartsActivityCorrectly()
        {
            // Arrange
            var gameLoopService = new GameLoopService();
            var activityMeta = new TimedActivityMeta { Duration = 5 };

            // Act
            gameLoopService.StartActivity(activityMeta);

            // Assert
            Assert.True(gameLoopService.CurrentActivityMeta != null);
            Assert.Equal(activityMeta, gameLoopService.CurrentActivityMeta);
        }

        [Fact]
        public async Task GameLoopService_InvokesOnGameTickEvent()
        {
            // Arrange
            var gameLoopService = new GameLoopService();
            var tickInvoked = false;
            gameLoopService.OnGameTick += () => tickInvoked = true;

            // Act
            await Task.Delay(50); // Allow some time for ticks to occur

            // Assert
            Assert.True(tickInvoked);
        }

        [Fact]
        public async Task GameLoopService_InvokesOnDayPassedEvent()
        {
            // Arrange
            var gameLoopService = new GameLoopService();
            var dayPassedInvoked = false;
            gameLoopService.OnDayPassed += () => dayPassedInvoked = true;
            var activityMeta = new TimedActivityMeta { Duration = 1 };
            gameLoopService.StartActivity(activityMeta);

            // Act
            await Task.Delay(500); // Allow some time for a day to pass

            // Assert
            Assert.True(dayPassedInvoked);
        }

        [Fact]
        public async Task GameLoopService_StopsActivityAfterDuration()
        {
            // Arrange
            var gameLoopService = new GameLoopService();
            var activityMeta = new TimedActivityMeta { Duration = 1 }; // Duration is 1 day
            gameLoopService.StartActivity(activityMeta);

            // Act
            await Task.Delay(2000); // Allow time for the activity to complete

            // Assert
            Assert.Null(gameLoopService.CurrentActivityMeta);
        }

        [Fact]
        public void GameLoopService_DisposesCorrectly()
        {
            // Arrange
            var gameLoopService = new GameLoopService();

            // Act
            gameLoopService.Dispose();

            // Assert
            // Ensure that calling Dispose twice does not throw an exception
            var exception = Record.Exception(() => gameLoopService.Dispose());
            Assert.Null(exception);

            // Ensure that the game loop does not tick after Dispose is called
            var tickCount = 0;
            gameLoopService.OnGameTick += () => tickCount++;

            // Wait some time to ensure no more ticks are happening
            System.Threading.Thread.Sleep(50);
            Assert.Equal(0, tickCount);
        }
    }
}

