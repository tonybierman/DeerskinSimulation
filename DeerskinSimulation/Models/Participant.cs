using System;
using System.Collections.Generic;
using DeerskinSimulation.Resources;

namespace DeerskinSimulation.Models
{
    public abstract class Participant
    {
        private int _skins;
        private double _money;
        private IRandomEventStrategy _huntingEventStrategy;
        private IRandomEventStrategy _forwardingEventStrategy;
        private IRandomEventStrategy _transportingEventStrategy;
        private IRandomEventStrategy _exportingEventStrategy;

        public string Name { get; }
        public int Skins => _skins;
        public double Money => _money;

        public event EventHandler<EventResult> OnNotification;

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
            _forwardingEventStrategy = new ForwardingEventStrategy();
            _transportingEventStrategy = new TransportingEventStrategy();
            _exportingEventStrategy = new ExportingEventStrategy();
        }

        protected virtual void RaiseNotification(string message, string color)
        {
            OnNotification?.Invoke(this, 
                new EventResult(new EventRecord($"{Name}: {message}", color)));
        }

        public virtual void AddSkins(int amount)
        {
            _skins += amount;
            //RaiseNotification($"Added {amount} skins.", "green");
        }

        public virtual void RemoveSkins(int amount)
        {
            if (_skins <= 0)
                return;

            _skins = Math.Max(0, _skins - amount);
            //RaiseNotification($"Removed {amount} skins.", "red");
        }

        public virtual void AddMoney(double amount)
        {
            _money += amount;
            //RaiseNotification($"Added ${amount:F2}.", "green");
        }

        public virtual void RemoveMoney(double amount)
        {
            if (_money <= 0)
                return;

            _money = Math.Max(0, _money - amount);
            //RaiseNotification($"Removed ${amount:F2}.", "red");
        }

        protected EventResult ApplyRandomEvent(IRandomEventStrategy eventStrategy)
        {
            var eventResult = eventStrategy.ApplyEvent(this);
            eventResult.ApplyActions(this);
            return eventResult;
        }

        protected EventResult ApplyRandomHuntingEvent()
        {
            return ApplyRandomEvent(_huntingEventStrategy);
        }

        protected EventResult ApplyRandomForwardingEvent()
        {
            return ApplyRandomEvent(_forwardingEventStrategy);
        }

        protected EventResult ApplyRandomTransportingEvent()
        {
            return ApplyRandomEvent(_transportingEventStrategy);
        }

        protected EventResult ApplyRandomExportingEvent()
        {
            return ApplyRandomEvent(_exportingEventStrategy);
        }

        public virtual EventResult TransportSkins(Participant recipient, int numberOfSkins, double transportCost, double pricePerSkin, double markup)
        {
            if (_skins < numberOfSkins)
            {
                return new EventResult(new EventRecord(Strings.NotEnoughSkinsToTransport));
            }

            double principal = CalculateTransactionCost(numberOfSkins, pricePerSkin);
            double totalCost = principal + transportCost;
            double sellingPrice = CalculateSellingPrice(totalCost, markup);

            if (recipient.Money < sellingPrice)
            {
                return new EventResult(new EventRecord(Strings.RecipientCannotAffordSkins));
            }

            recipient.RemoveMoney(sellingPrice);
            recipient.AddSkins(numberOfSkins);
            RemoveSkins(numberOfSkins);
            AddMoney(sellingPrice);

            var eventResult = ApplyRandomForwardingEvent();
            eventResult.Records.Add(new EventRecord($"Transported {numberOfSkins} skins."));

            return eventResult;
        }

        public virtual EventResult ExportSkins(int numberOfSkins, double exportCost, double duty, double pricePerSkin, double markup)
        {
            if (_skins < numberOfSkins)
            {
                return new EventResult(new EventRecord(Strings.NoSkinsToExport));
            }

            double principal = CalculateTransactionCost(numberOfSkins, pricePerSkin);
            double totalCost = CalculateTotalCost(principal, exportCost, duty);
            double sellingPrice = CalculateSellingPrice(principal + totalCost, markup);

            if (_money < totalCost)
            {
                return new EventResult(new EventRecord(Strings.NotEnoughMoneyToExport));
            }

            RemoveMoney(totalCost);
            AddMoney(sellingPrice);
            RemoveSkins(numberOfSkins);

            var eventResult = ApplyRandomExportingEvent();
            eventResult.Records.Add(new EventRecord($"Exported {numberOfSkins} skins."));
            
            return eventResult;
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
    }
}
