namespace DeerskinSimulation.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DeerskinSimulation.Commands;
    using DeerskinSimulation.Models;
    using DeerskinSimulation.Services;
    using Microsoft.AspNetCore.Components;

    public class SimulationViewModel
    {
        private StateContainer? _session;
        public HttpClient? Http { get; private set;}
        public bool Debug { get; private set; } 
        public RoleHunter HunterInstance { get; private set; }
        public RoleTrader TraderInstance { get; private set; }
        public RoleExporter ExporterInstance { get; private set; }
        public List<EventResult> Messages { get; private set; }
        public int SelectedPackhorses { get; set; }
        public UserInitiatedActivitySequence CurrentUserActivity { get; set; }
        public ConfirmForwardCommand ConfirmSellCmd { get; }
        public ConfirmHuntCommand ConfirmHuntCmd { get; }
        public ConfirmTransportCommand ConfirmTransportCmd { get; }
        public ConfirmExportCommand ConfirmExportCmd { get; }

        public event Func<Task> StateChanged;

        public SimulationViewModel(StateContainer? session, GameLoopService gameLoopService, HttpClient http)
        {
            Http = http;
            _session = session;
            Debug = _session?.Debug == true;

            ConfirmSellCmd = new ConfirmForwardCommand(this, gameLoopService);
            ConfirmHuntCmd = new ConfirmHuntCommand(this, gameLoopService);
            ConfirmTransportCmd = new ConfirmTransportCommand(this, gameLoopService);
            ConfirmExportCmd = new ConfirmExportCommand(this, gameLoopService);

            HunterInstance = new RoleHunter("Kanta-ke");
            TraderInstance = new RoleTrader("Bethabara");
            ExporterInstance = new RoleExporter("Charleston");

            Messages = new List<EventResult>();
            SelectedPackhorses = 1;

            HunterInstance.OnNotification += HandleNotification;
            TraderInstance.OnNotification += HandleNotification;
            ExporterInstance.OnNotification += HandleNotification;
        }

        public async Task UpdateUserActivityDay()
        {
            if (CurrentUserActivity.Meta == null) return;

            // First day of activity
            if (CurrentUserActivity.Meta.Elapsed == 0 && CurrentUserActivity.Start != null)
            {
                await CurrentUserActivity.Start.Invoke();
            }

            CurrentUserActivity.Meta.Elapsed++;

            // Last day of activity
            if (CurrentUserActivity.Meta.Elapsed >= CurrentUserActivity.Meta?.Duration)
            {
                if (CurrentUserActivity.Finish != null)
                {
                    await CurrentUserActivity.Finish.Invoke();
                }

                CurrentUserActivity = null;

                return;
            }

            // Every day in between first and last day
            if (CurrentUserActivity.InProcess != null)
            {
                await CurrentUserActivity.InProcess.Invoke();
            }
        }

        private async void HandleNotification(object sender, EventResult e)
        {
            if (e.HasRecords())
            {
                Messages.Add(e);
                await StateChanged?.Invoke();
            }
        }
    }
}
