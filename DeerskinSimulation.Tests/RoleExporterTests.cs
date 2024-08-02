using Xunit;
using Moq;
using DeerskinSimulation.Models;
using DeerskinSimulation.Resources;

namespace DeerskinSimulation.Tests
{
    public class RoleExporterTests
    {
        private readonly RoleExporter _exporter;
        private readonly Mock<IRandomEventStrategy> _mockExportingEventStrategy;

        public RoleExporterTests()
        {
            _mockExportingEventStrategy = new Mock<IRandomEventStrategy>();
            _exporter = new RoleExporter("TestExporter", 1000.0, 50, _mockExportingEventStrategy.Object);
        }

        [Fact]
        public void Export_ShouldFail_WhenNotEnoughSkins()
        {
            // Arrange
            int numberOfSkins = 60; // More than available skins

            // Act
            var result = _exporter.Export(numberOfSkins);

            // Assert
            Assert.Contains(result.Records, record => record.Message.Contains(Strings.NotEnoughSkinsToExport));
            Assert.Equal(EventResultStatus.Fail, result.Status);
        }

        [Fact]
        public void Export_ShouldSucceed_WhenEnoughSkins()
        {
            // Arrange
            int numberOfSkins = 10;

            // Setup mock to return a successful event result
            _mockExportingEventStrategy
                .Setup(s => s.ApplyEvent(It.IsAny<ParticipantRole>()))
                .Returns(new EventResult { Status = EventResultStatus.Success });

            // Act
            var result = _exporter.Export(numberOfSkins);

            // Assert
            Assert.Contains(result.Records, record => record.Message.Contains($"Exported {numberOfSkins} skins."));
            Assert.Equal(EventResultStatus.Success, result.Status);
        }

        [Fact]
        public void ExportSkins_ShouldFail_WhenNotEnoughSkins()
        {
            // Arrange
            int numberOfSkins = 60; // More than available skins

            // Act
            var result = _exporter.ExportSkins(numberOfSkins, Constants.TransatlanticTransportCost, Constants.ExportDuty, Constants.DeerSkinPricePerLb, Constants.ExporterMarkup);

            // Assert
            Assert.Contains(result.Records, record => record.Message.Contains(Strings.NoSkinsToExport));
            Assert.Equal(EventResultStatus.Fail, result.Status);
        }

        [Fact]
        public void ExportSkins_ShouldFail_WhenNotEnoughMoney()
        {
            // Arrange
            int numberOfSkins = 10;
            _exporter.RemoveMoney(_exporter.Money); // Set money to zero

            // Act
            var result = _exporter.ExportSkins(numberOfSkins, Constants.TransatlanticTransportCost, Constants.ExportDuty, Constants.DeerSkinPricePerLb, Constants.ExporterMarkup);

            // Assert
            Assert.Contains(result.Records, record => record.Message.Contains(Strings.NotEnoughMoneyToExport));
            Assert.Equal(EventResultStatus.Fail, result.Status);
        }

        [Fact]
        public void ExportSkins_ShouldSucceed_WhenEnoughResources()
        {
            // Arrange
            int numberOfSkins = 10;
            double initialMoney = _exporter.Money;
            double principal = MathUtils.CalculateTransactionCost(numberOfSkins, Constants.DeerSkinPricePerLb);
            double totalCost = MathUtils.CalculateTotalCost(principal, Constants.TransatlanticTransportCost, Constants.ExportDuty);
            double sellingPrice = MathUtils.CalculateSellingPrice(principal + totalCost, Constants.ExporterMarkup);

            // Setup mock to return a successful event result
            _mockExportingEventStrategy
                .Setup(s => s.ApplyEvent(It.IsAny<ParticipantRole>()))
                .Returns(new EventResult { Status = EventResultStatus.Success });

            // Act
            var result = _exporter.ExportSkins(numberOfSkins, Constants.TransatlanticTransportCost, Constants.ExportDuty, Constants.DeerSkinPricePerLb, Constants.ExporterMarkup);

            // Assert
            Assert.Contains(result.Records, record => record.Message.Contains($"Exported {numberOfSkins} skins."));
            Assert.Equal(EventResultStatus.Success, result.Status);
            Assert.Equal(initialMoney - totalCost + sellingPrice, _exporter.Money);
        }

        [Fact]
        public void RollForRandomExportingEvent_ShouldInvokeRandomEventStrategy()
        {
            // Arrange
            _mockExportingEventStrategy.Setup(s => s.ApplyEvent(It.IsAny<ParticipantRole>())).Returns(new EventResult());

            // Act
            var result = _exporter.RollForRandomExportingEvent();

            // Assert
            _mockExportingEventStrategy.Verify(s => s.ApplyEvent(_exporter), Times.Once);
        }
    }
}
