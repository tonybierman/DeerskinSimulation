namespace DeerskinSimulation.Commands
{
    using System.Threading.Tasks;
    using DeerskinSimulation.ViewModels;
    using DeerskinSimulation.Models;
    using DeerskinSimulation.Services;
    using DeerskinSimulation.Pages;
    using DeerskinSimulation.Resources;

    public class ConfirmHuntCommand
    {
        private readonly SimulationViewModel _viewModel;
        private readonly GameLoopService _gameLoopService;

        public ConfirmHuntCommand(SimulationViewModel viewModel, GameLoopService gameLoopService)
        {
            _viewModel = viewModel;
            _gameLoopService = gameLoopService;
        }

        public async Task ExecuteAsync(HuntOptionsViewModel sellOptions)
        {
            _viewModel.CurrentUserActivity = new UserActivity
            {
                Meta = new TimedActivityMeta { Name = Strings.HuntingActivityName, Duration = 10 },
                Start = async () =>
                {
                    await _viewModel.Hunt();
                },
                InProcess = async () =>
                {
                    await _viewModel.RandomHuntingEventCheck();
                },
                Finish = async () =>
                {
                    _viewModel.CurrentUserActivity = null;
                }
            };
            _gameLoopService.StartActivity(_viewModel.CurrentUserActivity.Meta);
        }
    }
}
