﻿namespace DeerskinSimulation.Commands
{
    using System.Threading.Tasks;
    using DeerskinSimulation.ViewModels;
    using DeerskinSimulation.Models;
    using DeerskinSimulation.Services;
    using DeerskinSimulation.Pages;
    using DeerskinSimulation.Resources;

    public class ConfirmHuntCommand
    {
        private readonly SimulationViewModel _viewModel;
        private readonly GameLoopService _gameLoopService;

        public ConfirmHuntCommand(SimulationViewModel viewModel, GameLoopService gameLoopService)
        {
            _viewModel = viewModel;
            _gameLoopService = gameLoopService;
        }

        public async Task ExecuteAsync(HuntOptionsViewModel sellOptions)
        {
            _viewModel.CurrentUserActivity = new UserInitiatedActivitySequence
            {
                Meta = new TimelapseActivityMeta { Name = Strings.HuntingActivityName, Duration = 30 },
                InProcess = async () =>
                {
                    if (_viewModel?.CurrentUserActivity?.Meta?.Status != EventResultStatus.Fail)
                    {
                        _viewModel.CurrentUserActivity.Meta.Status = await _viewModel.Hunt();
                        await _viewModel.RandomHuntingEventCheck();
                    }
                }
            };
            _gameLoopService.StartActivity(_viewModel.CurrentUserActivity.Meta);
        }
    }
}
