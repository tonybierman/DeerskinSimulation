namespace DeerskinSimulation.Commands
{
    using System.Threading.Tasks;
    using DeerskinSimulation.ViewModels;
    using DeerskinSimulation.Models;
    using DeerskinSimulation.Services;

    public class ConfirmTransportCommand
    {
        private readonly SimulationViewModel _viewModel;
        private readonly IGameLoopService _gameLoopService;
        private readonly ICommandFactory _commandFactory;
        private Trip _journey;

        public ConfirmTransportCommand(SimulationViewModel viewModel, IGameLoopService gameLoopService, ICommandFactory commandFactory)
        {
            _viewModel = viewModel;
            _gameLoopService = gameLoopService;
            _commandFactory = commandFactory;
        }

        public async Task ExecuteAsync(TransportOptionsViewModel transportOptions)
        {
            if (transportOptions.NumberOfSkins > 0)
            {
                if (_journey == null)
                {
                    //_journey = new Trip(_viewModel.Http, "data/bethabara_to_charleston_trip.json");
                    //await _journey.InitAsync();
                }

                var travelCommand = _commandFactory.CreateGreatWagonRoadCommand(_viewModel, _journey, transportOptions.NumberOfSkins);
                var deliverToExporterCommand = _commandFactory.CreateDeliverToExporterCommand(_viewModel, transportOptions.NumberOfSkins);
                var randomTransportEventCheckCommand = _commandFactory.CreateRandomTransportEventCheckCommand(_viewModel);

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
