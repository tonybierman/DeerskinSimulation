namespace DeerskinSimulation.Commands
{
    using System;
    using System.Threading.Tasks;
    using DeerskinSimulation.Models;
    using DeerskinSimulation.ViewModels;

    public class GreatWagonRoadCommand : ICommand
    {
        private readonly SimulationViewModel _viewModel;
        private readonly Trip _journey;
        private readonly int _numberOfSkins;

        public GreatWagonRoadCommand(SimulationViewModel viewModel, Trip journey, int numberOfSkins)
        {
            _numberOfSkins = numberOfSkins;
            _viewModel = viewModel;
            _journey = journey;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            // Implement logic to determine if the command can be executed.
            return true;
        }

        public EventResultStatus Execute(object? parameter)
        {
            // Implement synchronous execution logic if needed.
            throw new NotImplementedException();
        }

        public async Task<EventResultStatus> ExecuteAsync()
        {
            if (_viewModel.CurrentUserActivity?.Meta == null)
                throw new NullReferenceException(nameof(TimelapseActivityMeta));

            var meta = _viewModel.CurrentUserActivity.Meta;
            var result = _viewModel.Trader.TransportSkins(_viewModel, _viewModel.Exporter, _numberOfSkins);
            if (result.HasRecords())
            {
                result.Meta = new EventResultMeta(
                    meta.Duration,
                    meta.Elapsed,
                    meta.Name);

                _viewModel.AddMessage(result);
            }

            return result.Status;
        }

    }
}


