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
        private Trip _journey;

        public ConfirmTransportCommand(SimulationViewModel viewModel, GameLoopService gameLoopService)
        {
            _viewModel = viewModel;
            _gameLoopService = gameLoopService;
        }

        public async Task ExecuteAsync(TransportOptionsViewModel transportOptions)
        {
            if (transportOptions.NumberOfSkins > 0)
            {

                if (_journey == null)
                {
                    _journey = new Trip(_viewModel.Http, "data/bethabara_to_charleston_trip.json");
                    await _journey.InitAsync();
                }

                var travelCommand = new GreatWagonRoadCommand(_viewModel, _journey, transportOptions.NumberOfSkins);
                var deliverToExporterCommand = new DeliverToExporterCommand(_viewModel, transportOptions.NumberOfSkins);
                var randomTransportEventCheckCommand = new RandomTransportEventCheckCommand(_viewModel);

                _viewModel.CurrentUserActivity = new UserInitiatedActivitySequence
                {
                    Meta = new TimelapseActivityMeta { Name = "Wagontrain", Duration = 10 },
                    InProcess = async () =>
                    {
                        await travelCommand.ExecuteAsync();
                    },
                    Finish = async () =>
                    {
                        await randomTransportEventCheckCommand.ExecuteAsync();
                        await deliverToExporterCommand.ExecuteAsync();
                        _viewModel.CurrentUserActivity = null;
                    }
                };
                _gameLoopService.StartActivity(_viewModel.CurrentUserActivity.Meta);
            }
        }
    }
}
