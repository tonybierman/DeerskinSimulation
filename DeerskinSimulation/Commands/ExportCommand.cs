namespace DeerskinSimulation.Commands
{
    using System.Threading.Tasks;
    using DeerskinSimulation.ViewModels;
    using DeerskinSimulation.Models;

    public class ExportCommand : ICommand
    {
        private readonly SimulationViewModel _viewModel;
        private readonly int _numberOfSkins;

        public ExportCommand(SimulationViewModel viewModel, int numberOfSkins)
        {
            _viewModel = viewModel;
            _numberOfSkins = numberOfSkins;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            throw new NotImplementedException();
        }

        public async Task<EventResultStatus> ExecuteAsync()
        {
            var result = _viewModel.Exporter.Export(_numberOfSkins);
            if (result.HasRecords())
            {
                _viewModel.AddMessage(result);
            }

            return result.Status;
        }
    }
}
