﻿namespace DeerskinSimulation.Commands
{
    using System.Threading.Tasks;
    using DeerskinSimulation.ViewModels;
    using DeerskinSimulation.Models;
    using DeerskinSimulation.Services;
    using DeerskinSimulation.Pages;

    public class ConfirmTransportCommand
    {
        private readonly SimulationViewModel _viewModel;
        private readonly GameLoopService _gameLoopService;

        public ConfirmTransportCommand(SimulationViewModel viewModel, GameLoopService gameLoopService)
        {
            _viewModel = viewModel;
            _gameLoopService = gameLoopService;
        }

        public async Task ExecuteAsync(TransportOptionsViewModel transportOptions)
        {
            if (transportOptions.NumberOfSkins > 0)
            {
                _viewModel.CurrentUserActivity = new UserInitiatedActivitySequence
                {
                    Meta = new TimelapseActivityMeta { Name = "Transporting", Duration = 10 },
                    InProcess = async () =>
                    {
                        // TODO: Daily events on the road
                    },
                    Finish = async () =>
                    {
                        //await ViewModel.RandomTransportingEventCheck();
                        await _viewModel.TransportToExporter(transportOptions.NumberOfSkins);
                    }
                };
                _gameLoopService.StartActivity(_viewModel.CurrentUserActivity.Meta);
            }
        }
    }
}
