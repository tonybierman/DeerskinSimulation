﻿namespace DeerskinSimulation.Models
{
    using System;
    using DeerskinSimulation.Resources; // Make sure you have the appropriate using directive

    public class Hunter : Participant
    {
        public Hunter(string name) : base(name, Constants.HunterStartingFunds) { }

        public EventResult Hunt(int packhorses)
        {
            if (Money < Constants.HuntingCost)
            {
                return new EventResult(new EventRecord(Strings.NotEnoughMoneyToHunt));
            }

            RemoveMoney(Constants.HuntingCost);
            int skinsHunted = packhorses * Constants.PackhorseCapacity;
            AddSkins(skinsHunted);

            var eventMessage = ApplyRandomHuntingEvent();
            eventMessage.Records.Add(new EventRecord($"{Strings.HuntedSkins} {skinsHunted}."));

            return eventMessage;
        }

        public EventResult SellToTrader(Trader trader, int numberOfSkins)
        {
            if (Skins < numberOfSkins)
            {
                return new EventResult(new EventRecord(Strings.NotEnoughSkinsToSell));
            }

            double totalCost = numberOfSkins * Constants.DeerSkinPrice;
            trader.RemoveMoney(totalCost);
            trader.AddSkins(numberOfSkins);

            RemoveSkins(numberOfSkins);
            AddMoney(totalCost);

            var eventMessage = ApplyRandomTradingEvent();
            eventMessage.Records.Add(new EventRecord($"{Strings.SoldSkins} {numberOfSkins}."));

            return eventMessage;
        }
    }
}
