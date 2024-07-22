namespace DeerskinSimulation.Models
{
    using System;
    using DeerskinSimulation.Resources; // Make sure you have the appropriate using directive

    public class Hunter : Participant
    {
        public Hunter(string name) : base(name, Constants.HunterStartingFunds) { }

        public EventRecord Hunt(int packhorses)
        {
            if (Money < Constants.HuntingCost)
            {
                return new EventRecord(Strings.NotEnoughMoneyToHunt);
            }

            RemoveMoney(Constants.HuntingCost);
            int skinsHunted = packhorses * Constants.PackhorseCapacity;
            AddSkins(skinsHunted);

            var eventMessage = ApplyRandomHuntingEvent();
            eventMessage.Message = $"{Strings.HuntedSkins} {skinsHunted}. {eventMessage?.Message}";

            return eventMessage;
        }

        public EventRecord SellToTrader(Trader trader, int numberOfSkins)
        {
            if (Skins < numberOfSkins)
            {
                return new EventRecord(Strings.NotEnoughSkinsToSell);
            }

            double totalCost = numberOfSkins * Constants.DeerSkinPrice;
            trader.RemoveMoney(totalCost);
            trader.AddSkins(numberOfSkins);

            RemoveSkins(numberOfSkins);
            AddMoney(totalCost);

            var eventMessage = ApplyRandomTradingEvent();
            eventMessage.Message = $"{Strings.SoldSkins} {numberOfSkins}. {eventMessage?.Message}";

            return eventMessage;
        }
    }
}
