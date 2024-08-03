namespace DeerskinSimulation.Commands
{
    using System;
    using System.Threading.Tasks;
    using DeerskinSimulation.Models;
    using DeerskinSimulation.ViewModels;

    public class WildernessRoadCommand : ICommand
    {
        private readonly ISimulationViewModel _viewModel;

        public WildernessRoadCommand(ISimulationViewModel viewModel)
        {
            _viewModel = viewModel;
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

        public virtual async Task<EventResultStatus> ExecuteAsync()
        {
            if (_viewModel.CurrentUserActivity?.Meta == null)
                throw new NullReferenceException(nameof(TimelapseActivityMeta));

            var meta = _viewModel.CurrentUserActivity.Meta;

            var result = _viewModel.Hunter.Travel(_viewModel);
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


