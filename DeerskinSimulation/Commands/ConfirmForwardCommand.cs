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
                var forwardToTraderCommand = new ForwardToTraderCommand(_viewModel, sellOptions.NumberOfSkins);
                var randomForwardingEventCheckCommand = new RandomForwardingEventCheckCommand(_viewModel);

                _viewModel.CurrentUserActivity = new UserInitiatedActivitySequence
                {
                    Meta = new TimelapseActivityMeta { Name = "Forwarding", Duration = 14 },
                    InProcess = async () =>
                    {
                        // Execute the random forwarding event check command
                        await randomForwardingEventCheckCommand.ExecuteAsync();
                    },
                    Finish = async () =>
                    {
                        // Execute the forward to trader command
                        await forwardToTraderCommand.ExecuteAsync();
                        _viewModel.CurrentUserActivity = null;
                    }
                };

                _gameLoopService.StartActivity(_viewModel.CurrentUserActivity.Meta);
            }
        }
    }
}
