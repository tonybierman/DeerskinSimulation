﻿namespace DeerskinSimulation.Commands
{
    using System.Threading.Tasks;
    using DeerskinSimulation.ViewModels;
    using DeerskinSimulation.Models;

    public class RandomExportEventCheckCommand : ICommand
    {
        private readonly SimulationViewModel _viewModel;

        public RandomExportEventCheckCommand(SimulationViewModel viewModel)
        {
            _viewModel = viewModel;
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
            var result = _viewModel.ExporterInstance.RollForRandomExportingEvent();
            if (result.HasRecords())
            {
                _viewModel.Features.Add(result);
            }

            return result.Status;
        }
    }
}
