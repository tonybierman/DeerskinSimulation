namespace DeerskinSimulation.Commands
{
    using System.Threading.Tasks;
    using DeerskinSimulation.ViewModels;
    using DeerskinSimulation.Models;
    using DeerskinSimulation.Services;
    using DeerskinSimulation.Resources;

    public class ConfirmHuntCommand
    {
        private readonly SimulationViewModel _viewModel;
        private readonly IGameLoopService _gameLoopService;
        private readonly ICommandFactory _commandFactory;

        public ConfirmHuntCommand(SimulationViewModel viewModel, IGameLoopService gameLoopService, ICommandFactory commandFactory)
        {
            _viewModel = viewModel;
            _gameLoopService = gameLoopService;
            _commandFactory = commandFactory;
        }

        public async Task ExecuteAsync(HuntOptionsViewModel huntOptions)
        {
            _viewModel.CurrentUserActivity = new UserInitiatedActivitySequence
            {
                Meta = new TimelapseActivityMeta { Name = Strings.HuntingActivityName, Duration = 30 },
                InProcess = async () =>
                {
                    if (_viewModel?.CurrentUserActivity?.Meta?.Status != EventResultStatus.Fail)
                    {
                        var huntCommand = _commandFactory.CreateHuntCommand(_viewModel);
                        _viewModel.CurrentUserActivity.Meta.Status = await huntCommand.ExecuteAsync();
                        var randomHuntingEventCheckCommand = _commandFactory.CreateRandomHuntingEventCheckCommand(_viewModel);
                        await randomHuntingEventCheckCommand.ExecuteAsync();
                    }
                },
                Finish = async () =>
                {
                    var deliverToCampCommand = _commandFactory.CreateDeliverToCampCommand(_viewModel);
                    _viewModel.CurrentUserActivity.Meta.Status = await deliverToCampCommand.ExecuteAsync();
                }
            };
            _gameLoopService.StartActivity(_viewModel.CurrentUserActivity.Meta);
        }
    }
}
