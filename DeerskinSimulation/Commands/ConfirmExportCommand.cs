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
                _viewModel.CurrentUserActivity = new UserActivity
                {
                    Meta = new TimedActivityMeta { Name = "Exporting", Duration = 10 },
                    Start = async () =>
                    {
                        await _viewModel.Export(exportOptions.NumberOfSkins);
                    },
                    InProcess = async () =>
                    {
                        //TODO: await ViewModel.RandomExportingEventCheck();
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
}
