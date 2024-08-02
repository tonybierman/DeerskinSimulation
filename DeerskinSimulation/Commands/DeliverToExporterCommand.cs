﻿namespace DeerskinSimulation.Commands
{
    using System.Threading.Tasks;
    using DeerskinSimulation.ViewModels;
    using DeerskinSimulation.Models;

    public class DeliverToExporterCommand : ICommand
    {
        private readonly ISimulationViewModel _viewModel;
        private readonly int _numberOfSkins;

        public DeliverToExporterCommand(ISimulationViewModel viewModel, int numberOfSkins)
        {
            _viewModel = viewModel;
            _numberOfSkins = numberOfSkins;
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

        public async Task<EventResultStatus> ExecuteAsync()
        {
            var result = _viewModel.Trader.DeliverToExporter(_viewModel, _viewModel.Exporter, _numberOfSkins);
            if (result.HasRecords())
            {
                _viewModel.SetFeatured(result.LastRecord());
            }

            return result.Status;
        }
    }
}
