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
            OnNotification?.Invoke(this, new NotificationEventArgs(this, $"{Name}: {message}", color));
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

        protected string ApplyRandomEvent(IRandomEventStrategy eventStrategy)
        {
            return eventStrategy.ApplyEvent(this);
        }

        protected string ApplyRandomHuntingEvent()
        {
            return ApplyRandomEvent(_huntingEventStrategy);
        }

        protected string ApplyRandomTradingEvent()
        {
            return ApplyRandomEvent(_tradingEventStrategy);
        }

        protected string ApplyRandomTransportingEvent()
        {
            return ApplyRandomEvent(_transportingEventStrategy);
        }

        protected string ApplyRandomExportingEvent()
        {
            return ApplyRandomEvent(_exportingEventStrategy);
        }

        public virtual string Hunt(int packhorses)
        {
            return Strings.NotEnoughMoneyToHunt;
        }

        public virtual string SellSkins(Participant buyer, int numberOfSkins)
        {
            if (_skins < numberOfSkins)
            {
                return Strings.NoSkinsToSell;
            }

            double cost = CalculateTransactionCost(numberOfSkins, Constants.DeerSkinPrice);
            if (buyer.Money < cost)
            {
                return Strings.BuyerCannotAffordSkins;
            }

            ProcessTransaction(buyer, cost, numberOfSkins, AddMoney, AddSkins, RemoveSkins);
            return $"Sold {numberOfSkins} skins. {ApplyRandomTradingEvent()}";
        }

        public virtual string TransportSkins(Participant recipient, int numberOfSkins, double transportCost, double pricePerSkin, double markup)
        {
            if (_skins < numberOfSkins)
            {
                return Strings.NotEnoughSkinsToTransport;
            }

            double principal = CalculateTransactionCost(numberOfSkins, pricePerSkin);
            double totalCost = principal + transportCost;
            double sellingPrice = CalculateSellingPrice(totalCost, markup);

            if (recipient.Money < sellingPrice)
            {
                return Strings.RecipientCannotAffordSkins;
            }

            ProcessTransaction(recipient, sellingPrice, numberOfSkins, AddMoney, AddSkins, RemoveSkins, totalCost * markup);
            return $"Transported {numberOfSkins} skins. {ApplyRandomTransportingEvent()}";
        }

        public virtual string ExportSkins(int numberOfSkins, double exportCost, double duty, double pricePerSkin, double markup)
        {
            if (_skins < numberOfSkins)
            {
                return Strings.NoSkinsToExport;
            }

            double principal = CalculateTransactionCost(numberOfSkins, pricePerSkin);
            double totalCost = CalculateTotalCost(principal, exportCost, duty);
            double sellingPrice = CalculateSellingPrice(principal + totalCost, markup);

            if (_money < totalCost)
            {
                return Strings.NotEnoughMoneyToExport;
            }

            RemoveMoney(totalCost);
            AddMoney(sellingPrice);
            RemoveSkins(numberOfSkins);

            return $"Exported {numberOfSkins} skins. {ApplyRandomExportingEvent()}";
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
            string randomEvent = string.Empty;
            if (participant is Trader || participant is Exporter)
            {
                randomEvent = ApplyRandomTradingEvent();
                finalSkins -= GetLostSkinsFromEvent(randomEvent);
            }

            participant.RemoveMoney(amount);
            participant.AddSkins(finalSkins);
            removeSkins(skins); // Remove the original number of skins before applying losses
            addMoney(amount + additionalProfit);

            if (!string.IsNullOrEmpty(randomEvent))
            {
                RaiseNotification(randomEvent, "red");
            }
        }

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
