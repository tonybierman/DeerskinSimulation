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
        private int currentDay;

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
        //public int UserActivityDay { get => currentDay; set => currentDay = value; }

        public event Func<Task> StateChanged;

        public SimulationViewModel(StateContainer? session, GameLoopService gameLoopService)
        {
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

        #region Hunt
        public virtual async Task<EventResultStatus> Hunt()
        {
            var result = HunterInstance.Hunt(SelectedPackhorses);
            if (result.HasRecords())
            {
                Messages.Add(result);
                await StateChanged?.Invoke();
            }

            return result.Status;
        }

        public virtual async Task RandomHuntingEventCheck()
        {
            var result = HunterInstance.RollForRandomHuntingEvent();
            if (result.HasRecords())
            {
                Messages.Add(result);
                await StateChanged?.Invoke();
            }
        }
        #endregion

        #region Forward
        public virtual async Task ForwardToTrader(int numberOfSkins)
        {
            var result = HunterInstance.ForwardToTrader(TraderInstance, numberOfSkins);
            if (result.HasRecords())
            {
                Messages.Add(result);
                await StateChanged?.Invoke();
            }
        }

        public virtual async Task RandomForwardingEventCheck()
        {
            var result = HunterInstance.RollForRandomForwardingEvent();
            if (result.HasRecords())
            {
                Messages.Add(result);
                await StateChanged?.Invoke();
            }
        }
        #endregion

        #region Transport
        public virtual async Task TransportToExporter(int numberOfSkins)
        {
            var result = TraderInstance.TransportToExporter(ExporterInstance, numberOfSkins);
            if (result.HasRecords())
            {
                Messages.Add(result);
                await StateChanged?.Invoke();
            }
        }

        public async Task RandomTransportEventCheck()
        {
            var result = TraderInstance.RollForRandomTransportingEvent();
            if (result.HasRecords())
            {
                Messages.Add(result);
                await StateChanged?.Invoke();
            }
        }
        #endregion

        #region Export
        public virtual async Task Export(int numberOfSkins)
        {
            var result = ExporterInstance.Export(numberOfSkins);
            if (result.HasRecords())
            {
                Messages.Add(result);
                await StateChanged?.Invoke();
            }
        }
        public async Task RandomExportEventCheck()
        {
            var result = ExporterInstance.RollForRandomExportingEvent();
            if (result.HasRecords())
            {
                Messages.Add(result);
                await StateChanged?.Invoke();
            }
        }
        #endregion

        public async void UpdateUserActivityDay()
        {
            if (CurrentUserActivity == null) return;

            // First day of activity
            if (CurrentUserActivity.DaysElapsed == 0 && CurrentUserActivity.Start != null)
            {
                await CurrentUserActivity.Start.Invoke();
            }

            CurrentUserActivity.DaysElapsed++;

            // Last day of activity
            if (CurrentUserActivity.DaysElapsed >= CurrentUserActivity.Meta?.Duration)
            {
                if (CurrentUserActivity.Finish != null)
                {
                    await CurrentUserActivity.Finish.Invoke();
                }

                CurrentUserActivity.DaysElapsed = -1;
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
