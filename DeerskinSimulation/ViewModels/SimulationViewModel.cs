﻿namespace DeerskinSimulation.ViewModels
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
        public bool Debug { get; private set; } 

        public Hunter HunterInstance { get; private set; }
        public Trader TraderInstance { get; private set; }
        public Exporter ExporterInstance { get; private set; }
        public List<EventResult> Messages { get; private set; }
        public int SelectedPackhorses { get; set; }
        public UserActivity CurrentUserActivity { get; set; }
        public event Func<Task> StateChanged;
        public ConfirmForwardCommand ConfirmSellCmd { get; }
        public ConfirmHuntCommand ConfirmHuntCmd { get; }
        public SimulationViewModel(StateContainer? session, GameLoopService gameLoopService)
        {
            _session = session;
            Debug = _session?.Debug == true;
            ConfirmSellCmd = new ConfirmForwardCommand(this, gameLoopService);
            ConfirmHuntCmd = new ConfirmHuntCommand(this, gameLoopService);
            HunterInstance = new Hunter("Kanta-ke");
            TraderInstance = new Trader("Bethabara");
            ExporterInstance = new Exporter("Charleston");
            Messages = new List<EventResult>();
            SelectedPackhorses = 1;

            HunterInstance.OnNotification += HandleNotification;
            TraderInstance.OnNotification += HandleNotification;
            ExporterInstance.OnNotification += HandleNotification;
        }

        #region Hunt
        public async Task Hunt()
        {
            var result = HunterInstance.Hunt(SelectedPackhorses);
            if (result.HasRecords())
            {
                Messages.Add(result);
                await StateChanged?.Invoke();
            }
        }

        public async Task RandomHuntingEventCheck()
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
        public async Task ForwardToTrader(int numberOfSkins)
        {
            var result = HunterInstance.ForwardToTrader(TraderInstance, numberOfSkins);
            if (result.HasRecords())
            {
                Messages.Add(result);
                await StateChanged?.Invoke();
            }
        }

        public async Task RandomForwardingEventCheck()
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
        public async Task TransportToExporter(int numberOfSkins)
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
        public async Task Export(int numberOfSkins)
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
