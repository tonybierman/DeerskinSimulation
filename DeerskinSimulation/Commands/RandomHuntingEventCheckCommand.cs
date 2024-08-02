﻿namespace DeerskinSimulation.Commands
{
    using System;
    using System.Threading.Tasks;
    using DeerskinSimulation.ViewModels;
    using DeerskinSimulation.Models;

    public class RandomHuntingEventCheckCommand : ICommand
    {
        private readonly ISimulationViewModel _viewModel;

        public RandomHuntingEventCheckCommand(ISimulationViewModel viewModel)
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
            var result = _viewModel.Hunter.RollForRandomHuntingEvent();
            if (result.HasRecords())
            {
                if (_viewModel.CurrentUserActivity?.Meta != null)
                {
                    result.Meta = new EventResultMeta(
                        _viewModel.CurrentUserActivity.Meta.Duration,
                        _viewModel.CurrentUserActivity.Meta.Elapsed,
                        _viewModel.CurrentUserActivity.Meta.Name);
                }
                _viewModel.AddMessage(result);
                _viewModel.SetFeatured(result.LastRecord());
            }

            return result.Status;
        }
    }
}
