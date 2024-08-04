namespace DeerskinSimulation.Commands
{
    using System.Threading.Tasks;
    using DeerskinSimulation.ViewModels;
    using DeerskinSimulation.Models;

    public class ExportCommand : ICommand
    {
        private readonly ISimulationViewModel _viewModel;
        private readonly int _numberOfSkins;

        public ExportCommand(ISimulationViewModel viewModel, int numberOfSkins)
        {
            _viewModel = viewModel;
            _numberOfSkins = numberOfSkins;
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

        public virtual async Task<EventResultStatus> ExecuteAsync()
        {
            var result = _viewModel.Exporter.Export(_numberOfSkins);
            if (result.HasRecords())
            {
                _viewModel.SetFeatured(result.LastRecord());
                _viewModel.AddMessage(result);
            }

            return result.Status;
        }
    }
}
