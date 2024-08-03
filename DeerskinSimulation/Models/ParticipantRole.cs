using System;
using System.Collections.Generic;
using DeerskinSimulation.Resources;

namespace DeerskinSimulation.Models
{
    public abstract class ParticipantRole
    {
        protected int _skins;
        protected double _money;

        public string Name { get; init; }
        public int Skins => _skins;
        public double Money => _money;

        public int CurrentBag { get; set; }

        public event EventHandler<EventResult> OnNotification;

        protected ParticipantRole(string name, double initialMoney = 0, int initialSkins = 0)
        {
            Name = name;
            _money = initialMoney;
            _skins = initialSkins;
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

        public virtual bool RemoveSkins(int amount)
        {
            if (!HasSkins(amount))
                throw new ApplicationException("Insufficient supply.  Check for has skins first.");

            _skins -= amount;
            //RaiseNotification($"Removed {amount} skins.", "red");
            return true;
        }

        public virtual void AddMoney(double amount)
        {
            _money += amount;
            //RaiseNotification($"Added ${amount:F2}.", "green");
        }

        public virtual bool RemoveMoney(double amount)
        {
            if (!HasMoney(amount))
                throw new ApplicationException("Insufficient funds.  Check for has money first.");

            _money -= amount;
            //RaiseNotification($"Removed ${amount:F2}.", "red");
            return true;
        }

        public virtual bool HasSkins(int amount)
        {
            return _skins >= amount;
        }

        public virtual bool HasMoney(double amount)
        {
            return _money >= amount;
        }

        protected EventResult ApplyRandomEvent(IRandomEventStrategy eventStrategy)
        {
            var eventResult = eventStrategy.ApplyEvent(this);
            eventResult.ApplyActions(this);
            return eventResult;
        }
    }
}
