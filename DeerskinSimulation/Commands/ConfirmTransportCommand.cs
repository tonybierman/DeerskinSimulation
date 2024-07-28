namespace DeerskinSimulation.Commands
{
    using System.Threading.Tasks;
    using DeerskinSimulation.ViewModels;
    using DeerskinSimulation.Models;
    using DeerskinSimulation.Services;

    public class ConfirmTransportCommand
    {
        private readonly SimulationViewModel _viewModel;
        private readonly GameLoopService _gameLoopService;

        public ConfirmTransportCommand(SimulationViewModel viewModel, GameLoopService gameLoopService)
        {
            _viewModel = viewModel;
            _gameLoopService = gameLoopService;
        }

        public async Task ExecuteAsync(TransportOptionsViewModel transportOptions)
        {
            if (transportOptions.NumberOfSkins > 0)
            {
                var transportToExporterCommand = new TransportToExporterCommand(_viewModel, transportOptions.NumberOfSkins);
                var randomTransportEventCheckCommand = new RandomTransportEventCheckCommand(_viewModel);

                _viewModel.CurrentUserActivity = new UserInitiatedActivitySequence
                {
                    Meta = new TimelapseActivityMeta { Name = "Transporting", Duration = 10 },
                    InProcess = async () =>
                    {
                        await randomTransportEventCheckCommand.ExecuteAsync();
                    },
                    Finish = async () =>
                    {
                        await transportToExporterCommand.ExecuteAsync();
                        _viewModel.CurrentUserActivity = null;
                    }
                };
                _gameLoopService.StartActivity(_viewModel.CurrentUserActivity.Meta);
            }
        }
    }
}
