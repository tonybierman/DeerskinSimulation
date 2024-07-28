namespace DeerskinSimulation.Commands
{
    using System.Threading.Tasks;
    using DeerskinSimulation.ViewModels;
    using DeerskinSimulation.Models;

    public class TransportToExporterCommand : ICommand
    {
        private readonly SimulationViewModel _viewModel;
        private readonly int _numberOfSkins;

        public TransportToExporterCommand(SimulationViewModel viewModel, int numberOfSkins)
        {
            _viewModel = viewModel;
            _numberOfSkins = numberOfSkins;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true; // Can be extended with logic to enable/disable the command
        }

        public void Execute(object? parameter)
        {
            ExecuteAsync().Wait(); // Blocking wait; in real applications prefer async all the way
        }

        public async Task<EventResultStatus> ExecuteAsync()
        {
            var result = _viewModel.TraderInstance.TransportToExporter(_viewModel.ExporterInstance, _numberOfSkins);
            if (result.HasRecords())
            {
                _viewModel.Messages.Add(result);
            }

            return result.Status;
        }
    }
}
