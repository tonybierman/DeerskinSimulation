using System;
using DeerskinSimulation.Resources;

namespace DeerskinSimulation.Models
{
    public abstract class Participant
    {
        private int _skins;
        private double _money;
        private IRandomEventStrategy _huntingEventStrategy;
        private IRandomEventStrategy _tradingEventStrategy;
        private IRandomEventStrategy _transportingEventStrategy;
        private IRandomEventStrategy _exportingEventStrategy;

        public string Name { get; }
        public int Skins => _skins;
        public double Money => _money;

        public event EventHandler<NotificationEventArgs> OnNotification;

        protected Participant(string name, double initialMoney)
        {
            Name = name;
            _money = initialMoney;
            _skins = 0;

            InitializeEventStrategies();
        }

        private void InitializeEventStrategies()
        {
            _huntingEventStrategy = new HuntingEventStrategy();
            _tradingEventStrategy = new TradingEventStrategy();
            _transportingEventStrategy = new TransportingEventStrategy();
            _exportingEventStrategy = new ExportingEventStrategy();
        }

        protected virtual void RaiseNotification(string message, string color)
        {
            OnNotification?.Invoke(this, new NotificationEventArgs(
                new[] { new EventRecord($"{Name}: {message}", color) }, this));
        }

        public virtual void AddSkins(int amount)
        {
            _skins += amount;
            RaiseNotification($"Added {amount} skins.", "green");
        }

        public virtual void RemoveSkins(int amount)
        {
            _skins = Math.Max(0, _skins - amount);
            RaiseNotification($"Removed {amount} skins.", "red");
        }

        public virtual void AddMoney(double amount)
        {
            _money += amount;
            RaiseNotification($"Added ${amount:F2}.", "green");
        }

        public virtual void RemoveMoney(double amount)
        {
            _money = Math.Max(0, _money - amount);
            RaiseNotification($"Removed ${amount:F2}.", "red");
        }

        protected EventRecord ApplyRandomEvent(IRandomEventStrategy eventStrategy)
        {
            return eventStrategy.ApplyEvent(this);
        }

        protected EventRecord ApplyRandomHuntingEvent()
        {
            return ApplyRandomEvent(_huntingEventStrategy);
        }

        protected EventRecord ApplyRandomTradingEvent()
        {
            return ApplyRandomEvent(_tradingEventStrategy);
        }

        protected EventRecord ApplyRandomTransportingEvent()
        {
            return ApplyRandomEvent(_transportingEventStrategy);
        }

        protected EventRecord ApplyRandomExportingEvent()
        {
            return ApplyRandomEvent(_exportingEventStrategy);
        }

        public virtual EventRecord Hunt(int packhorses)
        {
            return new EventRecord(Strings.NotEnoughMoneyToHunt);
        }

        public virtual EventRecord SellSkins(Participant buyer, int numberOfSkins)
        {
            if (_skins < numberOfSkins)
            {
                return new EventRecord(Strings.NoSkinsToSell);
            }

            double cost = CalculateTransactionCost(numberOfSkins, Constants.DeerSkinPrice);
            if (buyer.Money < cost)
            {
                return new EventRecord(Strings.BuyerCannotAffordSkins);
            }

            ProcessTransaction(buyer, cost, numberOfSkins, AddMoney, AddSkins, RemoveSkins);
            return new EventRecord($"Sold {numberOfSkins} skins. {ApplyRandomTradingEvent()?.Message}");
        }

        public virtual EventRecord TransportSkins(Participant recipient, int numberOfSkins, double transportCost, double pricePerSkin, double markup)
        {
            if (_skins < numberOfSkins)
            {
                return new EventRecord(Strings.NotEnoughSkinsToTransport);
            }

            double principal = CalculateTransactionCost(numberOfSkins, pricePerSkin);
            double totalCost = principal + transportCost;
            double sellingPrice = CalculateSellingPrice(totalCost, markup);

            if (recipient.Money < sellingPrice)
            {
                return new EventRecord(Strings.RecipientCannotAffordSkins);
            }

            ProcessTransaction(recipient, sellingPrice, numberOfSkins, AddMoney, AddSkins, RemoveSkins, totalCost * markup);
            return new EventRecord($"Transported {numberOfSkins} skins. {ApplyRandomTransportingEvent()?.Message}");
        }

        public virtual EventRecord ExportSkins(int numberOfSkins, double exportCost, double duty, double pricePerSkin, double markup)
        {
            if (_skins < numberOfSkins)
            {
                return new EventRecord(Strings.NoSkinsToExport);
            }

            double principal = CalculateTransactionCost(numberOfSkins, pricePerSkin);
            double totalCost = CalculateTotalCost(principal, exportCost, duty);
            double sellingPrice = CalculateSellingPrice(principal + totalCost, markup);

            if (_money < totalCost)
            {
                return new EventRecord(Strings.NotEnoughMoneyToExport);
            }

            RemoveMoney(totalCost);
            AddMoney(sellingPrice);
            RemoveSkins(numberOfSkins);

            return new EventRecord($"Exported {numberOfSkins} skins. {ApplyRandomExportingEvent()?.Message}");
        }

        private double CalculateTransactionCost(int skins, double pricePerSkin)
        {
            return skins * pricePerSkin;
        }

        private double CalculateSellingPrice(double totalCost, double markup)
        {
            return totalCost + (totalCost * markup);
        }

        private double CalculateTotalCost(double principal, double extraCost, double duty)
        {
            return extraCost + (principal * duty);
        }

        private void ProcessTransaction(Participant participant, double amount, int skins, Action<double> addMoney, Action<int> addSkins, Action<int> removeSkins, double additionalProfit = 0)
        {
            int finalSkins = skins;
            var randomEvent = EventRecord.Empty;
            if (participant is Trader || participant is Exporter)
            {
                randomEvent = ApplyRandomTradingEvent();
                finalSkins -= GetLostSkinsFromEvent(randomEvent.Message);
            }

            participant.RemoveMoney(amount);
            participant.AddSkins(finalSkins);
            removeSkins(skins); // Remove the original number of skins before applying losses
            addMoney(amount + additionalProfit);

            if (!EventRecord.IsNullOrEmpty(randomEvent))
            {
                // TODO: Do this better
                RaiseNotification(randomEvent.Message, "red");
            }
        }

        // TODO: This is a hack fix this
        private int GetLostSkinsFromEvent(string eventMessage)
        {
            int lostSkins = 0;
            if (eventMessage.Contains("Lost"))
            {
                string[] words = eventMessage.Split(' ');
                foreach (string word in words)
                {
                    if (int.TryParse(word, out int result))
                    {
                        lostSkins = result;
                        break;
                    }
                }
            }
            return lostSkins;
        }
    }
}
