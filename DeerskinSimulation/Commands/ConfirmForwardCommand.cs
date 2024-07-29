namespace DeerskinSimulation.Commands
{
    using System.Threading.Tasks;
    using DeerskinSimulation.ViewModels;
    using DeerskinSimulation.Models;
    using DeerskinSimulation.Services;

    public class ConfirmForwardCommand
    {
        private readonly SimulationViewModel _viewModel;
        private readonly GameLoopService _gameLoopService;

        public ConfirmForwardCommand(SimulationViewModel viewModel, GameLoopService gameLoopService)
        {
            _viewModel = viewModel;
            _gameLoopService = gameLoopService;
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
                        // travel command
                        var travelCommand = new WildernessRoadCommand(_viewModel);
                        await travelCommand.ExecuteAsync();
                    },
                    Finish = async () =>
                    {
                        // Execute the random forwarding event check command
                        var randomForwardingEventCheckCommand = new RandomForwardingEventCheckCommand(_viewModel);
                        await randomForwardingEventCheckCommand.ExecuteAsync();

                        // Execute the forward to trader command
                        var forwardToTraderCommand = new ForwardToTraderCommand(_viewModel, sellOptions.NumberOfSkins);
                        await forwardToTraderCommand.ExecuteAsync();
                        _viewModel.CurrentUserActivity = null;
                    }
                };

                _gameLoopService.StartActivity(_viewModel.CurrentUserActivity.Meta);
            }
        }
    }
}
