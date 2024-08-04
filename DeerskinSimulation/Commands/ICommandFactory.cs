using DeerskinSimulation.Models;
using DeerskinSimulation.ViewModels;

namespace DeerskinSimulation.Commands
{
    public interface ICommandFactory
    {
        HighSeasCommand CreateHighSeasCommand(ISimulationViewModel viewModel, Trip journey, int numberOfSkins);
        GreatWagonRoadCommand CreateGreatWagonRoadCommand(ISimulationViewModel viewModel, Trip journey, int numberOfSkins);
        DeliverToExporterCommand CreateDeliverToExporterCommand(ISimulationViewModel viewModel, int numberOfSkins);
        RandomTransportEventCheckCommand CreateRandomTransportEventCheckCommand(ISimulationViewModel viewModel);
        WildernessRoadCommand CreateWildernessRoadCommand(ISimulationViewModel viewModel);
        ForwardToTraderCommand CreateForwardToTraderCommand(ISimulationViewModel viewModel, int numberOfSkins);
        RandomForwardingEventCheckCommand CreateRandomForwardingEventCheckCommand(ISimulationViewModel viewModel);
        ExportCommand CreateExportCommand(ISimulationViewModel viewModel, int numberOfSkins);
        RandomExportEventCheckCommand CreateRandomExportEventCheckCommand(ISimulationViewModel viewModel);
        HuntCommand CreateHuntCommand(ISimulationViewModel viewModel);
        RandomHuntingEventCheckCommand CreateRandomHuntingEventCheckCommand(ISimulationViewModel viewModel);
        DeliverToCampCommand CreateDeliverToCampCommand(ISimulationViewModel viewModel);
    }
}
