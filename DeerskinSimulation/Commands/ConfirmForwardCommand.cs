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
                _viewModel.CurrentUserActivity = new UserActivity
                {
                    Meta = new TimedActivityMeta { Name = "Forwarding", Duration = 10 },
                    Start = async () => { },
                    InProcess = async () =>
                    {
                        await _viewModel.RandomForwardingEventCheck();
                    },
                    Finish = async () =>
                    {
                        await _viewModel.ForwardToTrader(Math.Min(sellOptions.NumberOfSkins, _viewModel.HunterInstance.Skins));
                        _viewModel.CurrentUserActivity = null;
                    }
                };
                _gameLoopService.StartActivity(_viewModel.CurrentUserActivity.Meta);
            }
        }
    }
}
