namespace DeerskinSimulation.Models
{
    using System;
    using DeerskinSimulation.Resources; // Make sure you have the appropriate using directive

    public class Hunter : Participant
    {
        public Hunter(string name) : base(name, Constants.HunterStartingFunds) { }

        public string Hunt(int packhorses)
        {
            if (Money < Constants.HuntingCost)
            {
                return Strings.NotEnoughMoneyToHunt;
            }

            RemoveMoney(Constants.HuntingCost);
            int skinsHunted = packhorses * Constants.PackhorseCapacity;
            AddSkins(skinsHunted);

            string eventMessage = ApplyRandomHuntingEvent();

            return $"{Strings.HuntedSkins} {skinsHunted}. {eventMessage}";
        }

        public string SellToTrader(Trader trader, int numberOfSkins)
        {
            if (Skins < numberOfSkins)
            {
                return Strings.NotEnoughSkinsToSell;
            }

            double totalCost = numberOfSkins * Constants.DeerSkinPrice;
            trader.RemoveMoney(totalCost);
            trader.AddSkins(numberOfSkins);

            RemoveSkins(numberOfSkins);
            AddMoney(totalCost);

            string eventMessage = ApplyRandomTradingEvent();

            return $"{Strings.SoldSkins} {numberOfSkins}. {eventMessage}";
        }
    }
}
