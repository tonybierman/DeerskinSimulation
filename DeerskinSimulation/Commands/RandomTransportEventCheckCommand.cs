namespace DeerskinSimulation.Commands
{
    using System.Threading.Tasks;
    using DeerskinSimulation.ViewModels;
    using DeerskinSimulation.Models;

    public class RandomTransportEventCheckCommand : ICommand
    {
        private readonly ISimulationViewModel _viewModel;

        public RandomTransportEventCheckCommand(ISimulationViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true; // Can be extended with logic to enable/disable the command
        }

        public EventResultStatus Execute(object? parameter)
        {
            throw new NotImplementedException();
            // ExecuteAsync().Wait(); // Blocking wait; in real applications prefer async all the way
        }

        public virtual async Task<EventResultStatus> ExecuteAsync()
        {
            var result = _viewModel.Trader.RollForRandomTransportingEvent();
            if (result.HasRecords())
            {
                _viewModel.AddMessage(result);
                _viewModel.SetFeatured(result.LastRecord());
            }

            return result.Status;
        }
    }
}
