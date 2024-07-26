﻿namespace DeerskinSimulation.Models
{
    public class RandomEventHighDemand : EventResult
    {
        public RandomEventHighDemand(double bonusMoney) : base(
            new EventRecord($"High demand increased the selling price by {bonusMoney}.", "green", "images/packhorse_256.jpg"),
                    originator => originator.AddMoney(bonusMoney),
                    recipient => recipient?.RemoveMoney(bonusMoney))
        {
        }
    }
}
