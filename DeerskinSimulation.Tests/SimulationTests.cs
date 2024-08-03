using Bunit;
using Xunit;
using Moq;
using DeerskinSimulation.Pages;
using DeerskinSimulation.ViewModels;
using DeerskinSimulation.Models;
using DeerskinSimulation.Services;
using Microsoft.Extensions.DependencyInjection;

public class SimulationTests : TestContext
{
    private readonly Mock<ISimulationViewModel> _mockViewModel;
    private readonly Mock<IGameLoopService> _mockGameLoopService;
    private readonly Mock<IStateContainer> _mockSession;
    private readonly IRenderedComponent<Simulation> _component;

    public SimulationTests()
    {
        _mockViewModel = new Mock<ISimulationViewModel>();
        _mockGameLoopService = new Mock<IGameLoopService>();
        _mockSession = new Mock<IStateContainer>();

        // Setup default mock behaviors
        _mockViewModel.Setup(vm => vm.GameDay).Returns(1);
        _mockViewModel.Setup(vm => vm.Debug).Returns(false);
        _mockViewModel.Setup(vm => vm.Messages).Returns(new List<EventResult>());
        _mockViewModel.Setup(vm => vm.Hunter).Returns(new RoleHunter("Kanta-ke", 100, 10));
        _mockViewModel.Setup(vm => vm.Trader).Returns(new RoleTrader("Bethabara", 200, 20));
        _mockViewModel.Setup(vm => vm.Exporter).Returns(new RoleExporter("Charleston", 300, 30));
        _mockViewModel.Setup(vm => vm.CurrentUserActivity).Returns((UserInitiatedActivitySequence)null);

        Services.AddSingleton(_mockViewModel.Object);
        Services.AddSingleton(_mockGameLoopService.Object);
        Services.AddSingleton(_mockSession.Object);

        _component = RenderComponent<Simulation>();
    }

    [Fact]
    public void RendersInitialUICorrectly()
    {
        // Assert
        _component.Markup.Contains("Logistics");
        _component.Markup.Contains("Game Day: 1");
        _component.Markup.Contains("Kanta-ke");
        _component.Markup.Contains("Bethabara");
        _component.Markup.Contains("Charleston");
    }

    [Fact]
    public void DisplaysHunterInformation()
    {
        // Assert
        _component.Markup.Contains("Kanta-ke");
        _component.Markup.Contains("Skins: 10");
        _component.Markup.Contains("Money: $100.00");
    }

    [Fact]
    public void DisplaysTraderInformation()
    {
        // Assert
        _component.Markup.Contains("Bethabara");
        _component.Markup.Contains("Skins: 20");
        _component.Markup.Contains("Money: $200.00");
    }

    [Fact]
    public void DisplaysExporterInformation()
    {
        // Assert
        _component.Markup.Contains("Charleston");
        _component.Markup.Contains("Skins: 30");
        _component.Markup.Contains("Money: $300.00");
    }

    [Fact]
    public void HuntButtonOpensHuntOptions()
    {
        // Act
        var button = _component.Find("#hunt-button");
        button.Click();

        // Assert
        Assert.True(_component.Markup.Contains("Hunt Options"));
    }

    [Fact]
    public void ForwardButtonOpensForwardOptions()
    {
        // Act
        var button = _component.Find("#forward-button");
        button.Click();

        // Assert
        Assert.True(_component.Markup.Contains("Forward to Trading Post"));
    }

    [Fact]
    public void TransportButtonOpensTransportOptions()
    {
        // Act
        var button = _component.Find("#transport-button");
        button.Click();

        // Assert
        Assert.True(_component.Markup.Contains("Transport Options"));
    }

    [Fact]
    public void ExportButtonOpensExportOptions()
    {
        // Act
        var button = _component.Find("#export-button");
        button.Click();

        // Assert
        Assert.True(_component.Markup.Contains("Export Options"));
    }
}
