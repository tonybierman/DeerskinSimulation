﻿namespace DeerskinSimulation.Commands
{
    using System;
    using System.Threading.Tasks;
    using DeerskinSimulation.Models;
    using DeerskinSimulation.ViewModels;

    public class TravelWildernessRoadCommand : ICommand
    {
        private readonly ISimulationViewModel _viewModel;

        public TravelWildernessRoadCommand(ISimulationViewModel viewModel)
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

        public async Task<EventResultStatus> ExecuteAsync()
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


