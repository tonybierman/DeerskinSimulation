namespace DeerskinSimulation.Commands
{
    using System.Threading.Tasks;
    using DeerskinSimulation.ViewModels;
    using DeerskinSimulation.Models;
    using DeerskinSimulation.Services;

    public class ConfirmForwardCommand
    {
        private readonly ISimulationViewModel _viewModel;
        private readonly IGameLoopService _gameLoopService;
        private readonly ICommandFactory _commandFactory;

        public ConfirmForwardCommand(SimulationViewModel viewModel, IGameLoopService gameLoopService, ICommandFactory commandFactory)
        {
            _viewModel = viewModel;
            _gameLoopService = gameLoopService;
            _commandFactory = commandFactory;
        }

        public async Task ExecuteAsync(ForwardOptionsViewModel sellOptions)
        {
            if (sellOptions.NumberOfSkins > 0)
            {
                _viewModel.CurrentUserActivity = new UserInitiatedActivitySequence
                {
                    Meta = new TimelapseActivityMeta { Name = "Packtrain", Duration = 14 },
                    InProcess = async () =>
                    {
                        // Use the command factory to create the travel command
                        var travelCommand = _commandFactory.CreateWildernessRoadCommand(_viewModel);
                        await travelCommand.ExecuteAsync();
                    },
                    Finish = async () =>
                    {
                        // Use the command factory to create the random forwarding event check command
                        var randomForwardingEventCheckCommand = _commandFactory.CreateRandomForwardingEventCheckCommand(_viewModel);
                        await randomForwardingEventCheckCommand.ExecuteAsync();

                        // Use the command factory to create the forward to trader command
                        var forwardToTraderCommand = _commandFactory.CreateForwardToTraderCommand(_viewModel, sellOptions.NumberOfSkins);
                        await forwardToTraderCommand.ExecuteAsync();
                        _viewModel.CurrentUserActivity = null;
                    }
                };

                _gameLoopService.StartActivity(_viewModel.CurrentUserActivity.Meta);
            }
        }
    }
}
