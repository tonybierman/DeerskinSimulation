namespace DeerskinSimulation.Commands
{
    using System.Threading.Tasks;
    using DeerskinSimulation.ViewModels;
    using DeerskinSimulation.Models;
    using DeerskinSimulation.Services;

    public class ConfirmExportCommand
    {
        private readonly SimulationViewModel _viewModel;
        private readonly GameLoopService _gameLoopService;

        public ConfirmExportCommand(SimulationViewModel viewModel, GameLoopService gameLoopService)
        {
            _viewModel = viewModel;
            _gameLoopService = gameLoopService;
        }

        public async Task ExecuteAsync(ExportOptionsViewModel exportOptions)
        {
            if (exportOptions.NumberOfSkins > 0)
            {
                var exportCommand = CreateExportCommand(_viewModel, exportOptions.NumberOfSkins);
                var randomExportEventCheckCommand = CreateRandomExportEventCheckCommand(_viewModel);

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

        protected virtual ExportCommand CreateExportCommand(SimulationViewModel viewModel, int numberOfSkins)
        {
            return new ExportCommand(viewModel, numberOfSkins);
        }

        protected virtual RandomExportEventCheckCommand CreateRandomExportEventCheckCommand(SimulationViewModel viewModel)
        {
            return new RandomExportEventCheckCommand(viewModel);
        }
    }
}
