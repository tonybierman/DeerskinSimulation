using DeerskinSimulation.Resources;
using System;

namespace DeerskinSimulation.Models
{
    public class Hunter : Participant
    {
        public Hunter(string name) : base(name, Constants.HunterStartingFunds) { }

        public string Hunt(int packhorses)
        {
            if (Money < Constants.HuntingCost + packhorses * Constants.PackhorseCost)
            {
                return Strings.NotEnoughMoneyToHunt;
            }

            if (packhorses < 1 || packhorses > Constants.MaxPackhorses)
            {
                return Strings.InvalidNumberOfPackhorses;
            }

            RemoveMoney(Constants.HuntingCost + packhorses * Constants.PackhorseCost);
            int maxSkins = packhorses * Constants.PackhorseCapacity;
            int skinsGained = new Random().Next(maxSkins / 2, maxSkins + 1); // Random skins between half and full capacity
            AddSkins(skinsGained);
            return $"Hunted {skinsGained} skins with {packhorses} packhorses. {ApplyRandomHuntingEvent()}";
        }

        public string SellToTrader(Trader trader)
        {
            return SellSkins(trader, Constants.DeerSkinPrice);
        }
    }
}
