using System;
using System.Collections.Generic;
using DeerskinSimulation.Resources;

namespace DeerskinSimulation.Models
{
    public abstract class ParticipantRole
    {
        protected int _skins;
        protected double _money;

        public string Name { get; }
        public int Skins => _skins;
        public double Money => _money;

        public event EventHandler<EventResult> OnNotification;

        protected ParticipantRole(string name, double initialMoney = 0, int intialSkins = 0)
        {
            Name = name;
            _money = initialMoney;
            _skins = intialSkins;
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
    }
}
