namespace DeerskinSimulation.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using DeerskinSimulation.Commands;
    using DeerskinSimulation.Models;
    using DeerskinSimulation.Services;

    public class SimulationViewModel : ISimulationViewModel
    {
        private readonly ICommandFactory _commandFactory;
        private readonly IGameLoopService _gameLoop;

        private readonly List<EventResult> _messages = new List<EventResult>();

        public bool Debug { get; private set; }
        public RoleHunter Hunter { get; private set; }
        public RoleTrader Trader { get; private set; }
        public RoleExporter Exporter { get; private set; }

        public IReadOnlyList<EventResult> Messages => _messages.AsReadOnly();
        public Story Featured { get; private set; }
        public int SelectedPackhorses { get; set; }
        public virtual UserInitiatedActivitySequence CurrentUserActivity { get; set; }
        public int GameDay => _gameLoop.DaysPassed;

        public ConfirmForwardCommand ConfirmSellCmd { get; }
        public ConfirmHuntCommand ConfirmHuntCmd { get; }
        public ConfirmTransportCommand ConfirmTransportCmd { get; }
        public ConfirmExportCommand ConfirmExportCmd { get; }

        public event Func<Task> StateChanged;

        public event Action<EventResult> MessageAdded;
        public event Action MessagesCleared;

        public SimulationViewModel(
            IGameLoopService gameLoopService,
            ICommandFactory commandFactory,
            IStateContainer session)
        {
            _gameLoop = gameLoopService ?? throw new ArgumentNullException(nameof(gameLoopService));
            _commandFactory = commandFactory ?? throw new ArgumentNullException(nameof(commandFactory));

            Debug = session?.Debug == true;

            ConfirmSellCmd = new ConfirmForwardCommand(this, gameLoopService, commandFactory);
            ConfirmHuntCmd = new ConfirmHuntCommand(this, gameLoopService, commandFactory);
            ConfirmTransportCmd = new ConfirmTransportCommand(this, gameLoopService, commandFactory);
            ConfirmExportCmd = new ConfirmExportCommand(this, gameLoopService, commandFactory);

            Hunter = new RoleHunter("Kanta-ke", Constants.HunterStartingFunds, 0);
            Trader = new RoleTrader("Bethabara", Constants.TraderStartingFunds, Constants.TraderStartingSkins);
            Exporter = new RoleExporter("Charleston", Constants.ExporterStartingFunds, Constants.ExporterStartingSkins);
            SelectedPackhorses = 1;
        }

        public void SubscribeToNotifications()
        {
            Hunter.OnNotification += HandleNotification;
            Trader.OnNotification += HandleNotification;
            Exporter.OnNotification += HandleNotification;
        }

        public void UnsubscribeFromNotifications()
        {
            Hunter.OnNotification -= HandleNotification;
            Trader.OnNotification -= HandleNotification;
            Exporter.OnNotification -= HandleNotification;
        }

        public void AddMessage(EventResult message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            _messages.Add(message);
            OnMessageAdded(message);
        }

        public void ClearMessages()
        {
            _messages.Clear();
            OnMessagesCleared();
        }

        public void SetFeatured(EventRecord? result)
        {
            Featured = new Story(result);
        }

        public async Task UpdateUserActivityDay()
        {
            if (CurrentUserActivity.Meta == null) return;

            if (CurrentUserActivity.Meta.Elapsed == 0 && CurrentUserActivity.Start != null)
            {
                await CurrentUserActivity.Start.Invoke();
            }

            CurrentUserActivity.Meta.Elapsed++;

            if (CurrentUserActivity.Meta.Elapsed >= CurrentUserActivity.Meta?.Duration)
            {
                if (CurrentUserActivity.Finish != null)
                {
                    await CurrentUserActivity.Finish.Invoke();
                }

                CurrentUserActivity = null;
                return;
            }

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

        protected virtual void OnMessageAdded(EventResult message)
        {
            MessageAdded?.Invoke(message);
        }

        protected virtual void OnMessagesCleared()
        {
            MessagesCleared?.Invoke();
        }
    }
}
