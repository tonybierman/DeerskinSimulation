using Xunit;
using Moq;
using DeerskinSimulation.Models;
using DeerskinSimulation.Resources;
using DeerskinSimulation.ViewModels;

namespace DeerskinSimulation.Tests
{
    public class RoleHunterTests
    {
        private readonly Mock<IRandomEventStrategy> _mockHuntingStrategy;
        private readonly Mock<IRandomEventStrategy> _mockForwardingStrategy;
        private readonly Mock<ISimulationViewModel> _mockViewModel;
        private readonly RoleHunter _hunter;
        private readonly UserInitiatedActivitySequence _activitySequence;

        public RoleHunterTests()
        {
            _mockHuntingStrategy = new Mock<IRandomEventStrategy>();
            _mockForwardingStrategy = new Mock<IRandomEventStrategy>();
            _mockViewModel = new Mock<ISimulationViewModel>();
            _hunter = new RoleHunter("TestHunter", 1000, 10, _mockHuntingStrategy.Object, _mockForwardingStrategy.Object);

            // Set up a real instance of UserInitiatedActivitySequence with a TimelapseActivityMeta
            _activitySequence = new UserInitiatedActivitySequence
            {
                Meta = new TimelapseActivityMeta { Name = "TestActivity", Duration = 10, Elapsed = 0 }
            };
        }

        [Fact]
        public void Travel_ShouldFail_WhenNotEnoughMoney()
        {
            // Arrange
            _hunter.RemoveMoney(_hunter.Money); // Ensure no money
            _mockViewModel.Setup(v => v.SelectedPackhorses).Returns(2);
            _mockViewModel.Setup(v => v.CurrentUserActivity).Returns(_activitySequence);

            // Act
            var result = _hunter.Travel(_mockViewModel.Object);

            // Assert
            Assert.Equal(EventResultStatus.Fail, result.Status);
            Assert.Contains(result.Records, r => r.Message.Contains(Strings.NotEnoughMoneyToHunt));
        }

        [Fact]
        public void Travel_ShouldSucceed_WhenEnoughMoney()
        {
            // Arrange
            _mockViewModel.Setup(v => v.SelectedPackhorses).Returns(2);
            _mockViewModel.Setup(v => v.CurrentUserActivity).Returns(_activitySequence);

            // Act
            var result = _hunter.Travel(_mockViewModel.Object);

            // Assert
            Assert.Equal(EventResultStatus.Success, result.Status);
            Assert.Contains(result.Records, r => r.Message.Contains("Traveled about 20 miles."));
        }

        [Fact]
        public void Hunt_ShouldFail_WhenNotEnoughMoney()
        {
            // Arrange
            _hunter.RemoveMoney(_hunter.Money); // Ensure no money
            _mockViewModel.Setup(v => v.SelectedPackhorses).Returns(2);
            _mockViewModel.Setup(v => v.CurrentUserActivity).Returns(_activitySequence);

            // Act
            var result = _hunter.Hunt(_mockViewModel.Object);

            // Assert
            Assert.Equal(EventResultStatus.Fail, result.Status);
            Assert.Contains(result.Records, r => r.Message.Contains(Strings.NotEnoughMoneyToHunt));
        }

        [Fact]
        public void Hunt_ShouldSucceed_WhenEnoughMoney()
        {
            // Arrange
            _mockViewModel.Setup(v => v.SelectedPackhorses).Returns(2);
            _mockViewModel.Setup(v => v.CurrentUserActivity).Returns(_activitySequence);

            // Act
            var result = _hunter.Hunt(_mockViewModel.Object);

            // Assert
            Assert.Equal(EventResultStatus.Success, result.Status);
        }

        [Fact]
        public void DeliverToTrader_ShouldFail_WhenNotEnoughSkins()
        {
            // Arrange
            var trader = new RoleTrader("Trader", 1000, 0);
            _hunter.RemoveSkins(_hunter.Skins); // Ensure no skins

            // Act
            var result = _hunter.DeliverToTrader(trader, 5);

            // Assert
            Assert.Equal(EventResultStatus.Fail, result.Status);
            Assert.Contains(result.Records, r => r.Message.Contains(Strings.NotEnoughSkinsToSell));
        }

        [Fact]
        public void DeliverToTrader_ShouldFail_WhenTraderHasNotEnoughMoney()
        {
            // Arrange
            var trader = new RoleTrader("Trader", 0, 0); // Trader with no money

            // Act
            var result = _hunter.DeliverToTrader(trader, 5);

            // Assert
            Assert.Equal(EventResultStatus.Fail, result.Status);
        }

        [Fact]
        public void DeliverToTrader_ShouldSucceed_WhenEnoughResources()
        {
            // Arrange
            var trader = new RoleTrader("Trader", 1000, 0);

            // Act
            var result = _hunter.DeliverToTrader(trader, 5);

            // Assert
            Assert.Equal(EventResultStatus.Success, result.Status);
        }
    }
}
