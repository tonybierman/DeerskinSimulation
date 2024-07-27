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
                    Meta = new TimelapseActivityMeta { Name = "Forwarding", Duration = 14 },
                    InProcess = async () =>
                    {
                        // TODO: Daily events on the road
                    },
                    Finish = async () =>
                    {
                        await _viewModel.RandomForwardingEventCheck();
                        await _viewModel.ForwardToTrader(Math.Min(sellOptions.NumberOfSkins, _viewModel.HunterInstance.Skins));
                    }
                };
                _gameLoopService.StartActivity(_viewModel.CurrentUserActivity.Meta);
            }
        }
    }
}
