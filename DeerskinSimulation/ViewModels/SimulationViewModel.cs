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
        public RoleHunter Hunter { get; private set; }
        public RoleTrader Trader { get; private set; }
        public RoleExporter Exporter { get; private set; }

        private readonly List<EventResult> _messages = new List<EventResult>();
        public IReadOnlyList<EventResult> Messages => _messages.AsReadOnly();

        public Story Featured { get; private set; }
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
            Hunter = new RoleHunter("Kanta-ke");
            Trader = new RoleTrader("Bethabara");
            Exporter = new RoleExporter("Charleston");
            SelectedPackhorses = 1;
            Hunter.OnNotification += HandleNotification;
            Trader.OnNotification += HandleNotification;
            Exporter.OnNotification += HandleNotification;
        }

        #region Messages

        // Event for external classes to subscribe to
        public event Action<EventResult> MessageAdded;
        public event Action MessagesCleared;

        protected virtual void OnMessageAdded(EventResult message)
        {
            MessageAdded?.Invoke(message);
        }

        protected virtual void OnMessagesCleared()
        {
            MessagesCleared?.Invoke();
        }

        public void ClearMessages()
        {
            _messages.Clear();
            OnMessagesCleared(); // Notify subscribers if needed
        }

        public void AddMessage(EventResult message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            // Add any validation or processing logic here
            _messages.Add(message);
            OnMessageAdded(message); // Notify subscribers if needed
        }

        #endregion

        public void SetFeatured(EventRecord? result)
        {
            Featured = new Story(result);
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
                AddMessage(e);
                await StateChanged?.Invoke();
            }
        }
    }
}
