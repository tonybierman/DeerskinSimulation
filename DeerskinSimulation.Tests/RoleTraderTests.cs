using Xunit;
using Moq;
using DeerskinSimulation.Models;
using DeerskinSimulation.Resources;
using DeerskinSimulation.ViewModels;

namespace DeerskinSimulation.Tests
{
    public class RoleTraderTests
    {
        private readonly Mock<ISimulationViewModel> _mockViewModel;
        private readonly RoleTrader _trader;
        private readonly RoleExporter _exporter;

        public RoleTraderTests()
        {
            _mockViewModel = new Mock<ISimulationViewModel>();
            _mockViewModel.Setup(vm => vm.CurrentUserActivity).Returns(new UserInitiatedActivitySequence
            {
                Meta = new TimelapseActivityMeta { Name = "TestActivity", Duration = 5 }
            });

            _trader = new RoleTrader("Trader", 1000, 50);
            _exporter = new RoleExporter("Exporter", 500, 0);
        }

        [Fact]
        public void DeliverToExporter_ShouldSucceed_WhenEnoughResources()
        {
            // Arrange
            int numberOfSkins = 10;
            double sellingPrice = MathUtils.CalculateTransactionCost(numberOfSkins,
                Constants.DeerSkinPricePerLb * Constants.DeerSkinWeightInLb * Constants.TraderMarkup);

            // Act
            var result = _trader.DeliverToExporter(_mockViewModel.Object, _exporter, numberOfSkins);

            // Assert
            Assert.Equal(EventResultStatus.Success, result.Status);
            Assert.Contains(result.Records, r => r.Message.Contains($"Delivered {numberOfSkins} skins to exporter."));
            Assert.Equal(40, _trader.Skins);  // 50 - 10
            Assert.Equal(10, _exporter.Skins); // 0 + 10
            Assert.Equal(1000 + sellingPrice, _trader.Money); // Trader money should increase by selling price
            Assert.Equal(500 - sellingPrice, _exporter.Money); // Exporter money should decrease by selling price
        }

        [Fact]
        public void DeliverToExporter_ShouldFail_WhenNotEnoughSkins()
        {
            // Arrange
            int numberOfSkins = 60; // More than the trader has

            // Act
            var result = _trader.DeliverToExporter(_mockViewModel.Object, _exporter, numberOfSkins);

            // Assert
            Assert.Equal(EventResultStatus.Fail, result.Status);
            Assert.Contains(result.Records, r => r.Message.Contains("Not enough skins to deliver."));
        }

        [Fact]
        public void DeliverToExporter_ShouldFail_WhenExporterHasInsufficientFunds()
        {
            // Arrange
            _exporter.RemoveMoney(_exporter.Money); // Ensure exporter has no money
            int numberOfSkins = 10;

            // Act
            var result = _trader.DeliverToExporter(_mockViewModel.Object, _exporter, numberOfSkins);

            // Assert
            Assert.Equal(EventResultStatus.Fail, result.Status);
            Assert.Contains(result.Records, r => r.Message.Contains("Exporter does not have enough money to complete the transaction."));
        }

        [Fact]
        public void TransportSkins_ShouldSucceed_WhenEnoughResources()
        {
            // Arrange
            int numberOfSkins = 10;
            double netCostPerDay = Constants.RegionalTransportCost * numberOfSkins;

            // Act
            var result = _trader.TransportSkins(_mockViewModel.Object, _exporter, numberOfSkins);

            // Assert
            Assert.Equal(EventResultStatus.Success, result.Status);
            Assert.Contains(result.Records, r => r.Message.Contains($"Transported {numberOfSkins} skins about 20 miles."));
            Assert.Equal(40, _trader.Skins);
            Assert.Equal(1000 - netCostPerDay, _trader.Money);
        }

        [Fact]
        public void TransportSkins_ShouldFail_WhenNotEnoughMoney()
        {
            // Arrange
            _trader.RemoveMoney(_trader.Money); // Ensure trader has no money
            int numberOfSkins = 10;

            // Act
            var result = _trader.TransportSkins(_mockViewModel.Object, _exporter, numberOfSkins);

            // Assert
            Assert.Equal(EventResultStatus.Fail, result.Status);
            Assert.Contains(result.Records, r => r.Message.Contains(Strings.NotEnoughMoneyTravel));
        }

        [Fact]
        public void TransportSkins_ShouldFail_WhenNotEnoughSkins()
        {
            // Arrange
            int numberOfSkins = 60; // More than the trader has

            // Act
            var result = _trader.TransportSkins(_mockViewModel.Object, _exporter, numberOfSkins);

            // Assert
            Assert.Equal(EventResultStatus.Fail, result.Status);
            Assert.Contains(result.Records, r => r.Message.Contains(Strings.NotEnoughSkinsToTransport));
        }
    }
}
