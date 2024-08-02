namespace DeerskinSimulation.Commands
{
    using System.Threading.Tasks;
    using DeerskinSimulation.ViewModels;
    using DeerskinSimulation.Models;
    using DeerskinSimulation.Services;

    public class ExportSequenceCoordinator
    {
        private readonly ISimulationViewModel _viewModel;
        private readonly IGameLoopService _gameLoopService;
        private readonly ICommandFactory _commandFactory;

        public ExportSequenceCoordinator(SimulationViewModel viewModel, IGameLoopService gameLoopService, ICommandFactory commandFactory)
        {
            _viewModel = viewModel;
            _gameLoopService = gameLoopService;
            _commandFactory = commandFactory;
        }

        public async Task ExecuteAsync(ExportOptionsViewModel exportOptions)
        {
            if (exportOptions.NumberOfSkins > 0)
            {
                var exportCommand = _commandFactory.CreateExportCommand(_viewModel, exportOptions.NumberOfSkins);
                var randomExportEventCheckCommand = _commandFactory.CreateRandomExportEventCheckCommand(_viewModel);

                _viewModel.CurrentUserActivity = new UserInitiatedActivitySequence
                {
                    Meta = new TimelapseActivityMeta { Name = "Exporting", Duration = 10 },
                    InProcess = async () =>
                    {
                        await randomExportEventCheckCommand.ExecuteAsync();
                    },
                    Finish = async () =>
                    {
                        await exportCommand.ExecuteAsync();
                        _viewModel.CurrentUserActivity = null;
                    }
                };
                _gameLoopService.StartActivity(_viewModel.CurrentUserActivity.Meta);
            }
        }
    }
}
