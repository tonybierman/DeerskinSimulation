namespace DeerskinSimulation.Commands
{
    using System.Threading.Tasks;
    using DeerskinSimulation.ViewModels;
    using DeerskinSimulation.Models;

    public class RandomTransportEventCheckCommand : ICommand
    {
        private readonly SimulationViewModel _viewModel;

        public RandomTransportEventCheckCommand(SimulationViewModel viewModel)
        {
            _viewModel = viewModel;
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
            var result = _viewModel.TraderInstance.RollForRandomTransportingEvent();
            if (result.HasRecords())
            {
                _viewModel.Features.Add(result);
            }

            return result.Status;
        }
    }
}
