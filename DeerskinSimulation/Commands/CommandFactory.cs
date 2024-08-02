using DeerskinSimulation.ViewModels;
using DeerskinSimulation.Models;
using DeerskinSimulation.Services;

namespace DeerskinSimulation.Commands
{

    public class CommandFactory : ICommandFactory
    {
        public GreatWagonRoadCommand CreateGreatWagonRoadCommand(ISimulationViewModel viewModel, Trip journey, int numberOfSkins)
        {
            return new GreatWagonRoadCommand(viewModel, journey, numberOfSkins);
        }

        public DeliverToExporterCommand CreateDeliverToExporterCommand(ISimulationViewModel viewModel, int numberOfSkins)
        {
            return new DeliverToExporterCommand(viewModel, numberOfSkins);
        }

        public RandomTransportEventCheckCommand CreateRandomTransportEventCheckCommand(ISimulationViewModel viewModel)
        {
            return new RandomTransportEventCheckCommand(viewModel);
        }

        public WildernessRoadCommand CreateWildernessRoadCommand(ISimulationViewModel viewModel)
        {
            return new WildernessRoadCommand(viewModel);
        }

        public ForwardToTraderCommand CreateForwardToTraderCommand(ISimulationViewModel viewModel, int numberOfSkins)
        {
            return new ForwardToTraderCommand(viewModel, numberOfSkins);
        }

        public RandomForwardingEventCheckCommand CreateRandomForwardingEventCheckCommand(ISimulationViewModel viewModel)
        {
            return new RandomForwardingEventCheckCommand(viewModel);
        }

        public ExportCommand CreateExportCommand(ISimulationViewModel viewModel, int numberOfSkins)
        {
            return new ExportCommand(viewModel, numberOfSkins);
        }

        public RandomExportEventCheckCommand CreateRandomExportEventCheckCommand(ISimulationViewModel viewModel)
        {
            return new RandomExportEventCheckCommand(viewModel);
        }

        public HuntCommand CreateHuntCommand(ISimulationViewModel viewModel)
        {
            return new HuntCommand(viewModel);
        }

        public RandomHuntingEventCheckCommand CreateRandomHuntingEventCheckCommand(ISimulationViewModel viewModel)
        {
            return new RandomHuntingEventCheckCommand(viewModel);
        }

        public DeliverToCampCommand CreateDeliverToCampCommand(ISimulationViewModel viewModel)
        {
            return new DeliverToCampCommand(viewModel);
        }
    }
}
