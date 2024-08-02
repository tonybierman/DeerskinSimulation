using DeerskinSimulation.Models;
using DeerskinSimulation.ViewModels;

namespace DeerskinSimulation.Commands
{
    public interface ICommandFactory
    {
        GreatWagonRoadCommand CreateGreatWagonRoadCommand(SimulationViewModel viewModel, Trip journey, int numberOfSkins);
        DeliverToExporterCommand CreateDeliverToExporterCommand(SimulationViewModel viewModel, int numberOfSkins);
        RandomTransportEventCheckCommand CreateRandomTransportEventCheckCommand(SimulationViewModel viewModel);
        WildernessRoadCommand CreateWildernessRoadCommand(SimulationViewModel viewModel);
        ForwardToTraderCommand CreateForwardToTraderCommand(SimulationViewModel viewModel, int numberOfSkins);
        RandomForwardingEventCheckCommand CreateRandomForwardingEventCheckCommand(SimulationViewModel viewModel);
        ExportCommand CreateExportCommand(SimulationViewModel viewModel, int numberOfSkins);
        RandomExportEventCheckCommand CreateRandomExportEventCheckCommand(SimulationViewModel viewModel);
        HuntCommand CreateHuntCommand(SimulationViewModel viewModel);
        RandomHuntingEventCheckCommand CreateRandomHuntingEventCheckCommand(SimulationViewModel viewModel);
        DeliverToCampCommand CreateDeliverToCampCommand(SimulationViewModel viewModel);
    }
}
