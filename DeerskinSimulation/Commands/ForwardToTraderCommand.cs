﻿namespace DeerskinSimulation.Commands
{
    using System;
    using System.Threading.Tasks;
    using DeerskinSimulation.Models;
    using DeerskinSimulation.ViewModels;

    public class ForwardToTraderCommand : ICommand
    {
        private readonly ISimulationViewModel _viewModel;
        private readonly int _numberOfSkins;

        public ForwardToTraderCommand(ISimulationViewModel viewModel, int numberOfSkins)
        {
            _viewModel = viewModel;
            _numberOfSkins = numberOfSkins;
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
            var result = _viewModel.Hunter.DeliverToTrader(_viewModel.Trader, _numberOfSkins);
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
