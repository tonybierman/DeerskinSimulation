using Xunit;
using Moq;
using DeerskinSimulation.Models;
using DeerskinSimulation.Resources;

namespace DeerskinSimulation.Tests;

public class RoleExporterTests
{
    private readonly RoleExporter _exporter;
    private readonly Mock<IRandomEventStrategy> _mockExportingEventStrategy;

    public RoleExporterTests()
    {
        _exporter = new RoleExporter("TestExporter");
        _mockExportingEventStrategy = new Mock<IRandomEventStrategy>();
    }

    [Fact]
    public void Export_ShouldFail_WhenNotEnoughSkins()
    {
        // Arrange
        int numberOfSkins = 10;
        _exporter.RemoveSkins(_exporter.Skins); // Ensure no skins available

        // Act
        var result = _exporter.Export(numberOfSkins);

        // Assert
        Assert.Contains(result.Records, record => record.Message.Contains(Strings.NotEnoughSkinsToExport));
        Assert.Equal(EventResultStatus.Fail, result.Status);
    }

    //[Fact]
    //public void Export_ShouldSucceed_WhenEnoughSkins()
    //{
    //    // Arrange
    //    int numberOfSkins = 10;
    //    _exporter.AddSkins(numberOfSkins);

    //    // Act
    //    var result = _exporter.Export(numberOfSkins);

    //    // Assert
    //    Assert.Contains(result.Records, record => record.Message.Contains($"Exported {numberOfSkins} skins."));
    //    Assert.Equal(EventResultStatus.Success, result.Status);
    //}

    //[Fact]
    //public void RollForRandomExportingEvent_ShouldInvokeRandomEventStrategy()
    //{
    //    // Arrange
    //    _mockExportingEventStrategy.Setup(s => s.ApplyEvent(It.IsAny<ParticipantRole>())).Returns(new EventResult());

    //    // Act
    //    var result = _exporter.RollForRandomExportingEvent();

    //    // Assert
    //    _mockExportingEventStrategy.Verify(s => s.ApplyEvent(_exporter), Times.Once);
    //}

    [Fact]
    public void ExportSkins_ShouldFail_WhenNotEnoughSkins()
    {
        // Arrange
        int numberOfSkins = 10;
        _exporter.RemoveSkins(_exporter.Skins); // Ensure no skins available

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
        _exporter.AddSkins(numberOfSkins);
        _exporter.RemoveMoney(_exporter.Money); // Ensure no money

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
        _exporter.AddSkins(numberOfSkins);
        _exporter.AddMoney(1000); // Ensure enough money

        // Act
        var result = _exporter.ExportSkins(numberOfSkins, Constants.TransatlanticTransportCost, Constants.ExportDuty, Constants.DeerSkinPricePerLb, Constants.ExporterMarkup);

        // Assert
        Assert.Contains(result.Records, record => record.Message.Contains($"Exported {numberOfSkins} skins."));
        Assert.Equal(EventResultStatus.Success, result.Status);
    }
}
