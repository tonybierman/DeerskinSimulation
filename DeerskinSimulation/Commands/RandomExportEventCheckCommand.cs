namespace DeerskinSimulation.Commands
{
    using System.Threading.Tasks;
    using DeerskinSimulation.ViewModels;
    using DeerskinSimulation.Models;

    public class RandomExportEventCheckCommand : ICommand
    {
        private readonly ISimulationViewModel _viewModel;

        public RandomExportEventCheckCommand(ISimulationViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public EventResultStatus Execute(object? parameter)
        {
            throw new NotImplementedException();
        }

        public async Task<EventResultStatus> ExecuteAsync()
        {
            var result = _viewModel.Exporter.RollForRandomExportingEvent();
            if (result.HasRecords())
            {
                _viewModel.AddMessage(result);
                _viewModel.SetFeatured(result.LastRecord());
            }

            return result.Status;
        }
    }
}
