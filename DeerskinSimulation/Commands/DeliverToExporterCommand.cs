namespace DeerskinSimulation.Commands
{
    using System.Threading.Tasks;
    using DeerskinSimulation.ViewModels;
    using DeerskinSimulation.Models;

    public class DeliverToExporterCommand : ICommand
    {
        private readonly SimulationViewModel _viewModel;
        private readonly int _numberOfSkins;

        public DeliverToExporterCommand(SimulationViewModel viewModel, int numberOfSkins)
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
            var result = _viewModel.Trader.DeliverToExporter(_viewModel, _viewModel.Exporter, _numberOfSkins);
            if (result.HasRecords())
            {
                _viewModel.SetFeatured(result.Records[0]);
            }

            return result.Status;
        }
    }
}
