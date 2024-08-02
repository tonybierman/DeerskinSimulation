using DeerskinSimulation.ViewModels;
using DeerskinSimulation.Models;
using DeerskinSimulation.Services;

namespace DeerskinSimulation.Commands
{

    public class CommandFactory : ICommandFactory
    {
        public GreatWagonRoadCommand CreateGreatWagonRoadCommand(SimulationViewModel viewModel, Trip journey, int numberOfSkins)
        {
            return new GreatWagonRoadCommand(viewModel, journey, numberOfSkins);
        }

        public DeliverToExporterCommand CreateDeliverToExporterCommand(SimulationViewModel viewModel, int numberOfSkins)
        {
            return new DeliverToExporterCommand(viewModel, numberOfSkins);
        }

        public RandomTransportEventCheckCommand CreateRandomTransportEventCheckCommand(SimulationViewModel viewModel)
        {
            return new RandomTransportEventCheckCommand(viewModel);
        }

        public WildernessRoadCommand CreateWildernessRoadCommand(SimulationViewModel viewModel)
        {
            return new WildernessRoadCommand(viewModel);
        }

        public ForwardToTraderCommand CreateForwardToTraderCommand(SimulationViewModel viewModel, int numberOfSkins)
        {
            return new ForwardToTraderCommand(viewModel, numberOfSkins);
        }

        public RandomForwardingEventCheckCommand CreateRandomForwardingEventCheckCommand(SimulationViewModel viewModel)
        {
            return new RandomForwardingEventCheckCommand(viewModel);
        }

        public ExportCommand CreateExportCommand(SimulationViewModel viewModel, int numberOfSkins)
        {
            return new ExportCommand(viewModel, numberOfSkins);
        }

        public RandomExportEventCheckCommand CreateRandomExportEventCheckCommand(SimulationViewModel viewModel)
        {
            return new RandomExportEventCheckCommand(viewModel);
        }

        public HuntCommand CreateHuntCommand(SimulationViewModel viewModel)
        {
            return new HuntCommand(viewModel);
        }

        public RandomHuntingEventCheckCommand CreateRandomHuntingEventCheckCommand(SimulationViewModel viewModel)
        {
            return new RandomHuntingEventCheckCommand(viewModel);
        }

        public DeliverToCampCommand CreateDeliverToCampCommand(SimulationViewModel viewModel)
        {
            return new DeliverToCampCommand(viewModel);
        }
    }
}
