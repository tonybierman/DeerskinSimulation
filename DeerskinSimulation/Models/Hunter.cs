using DeerskinSimulation.Resources;

namespace DeerskinSimulation.Models
{
    public class Hunter : Participant
    {
        public Hunter(string name) : base(name, Constants.HunterStartingFunds) { }

        public override string Hunt()
        {
            if (Money < Constants.HuntingCost)
            {
                return Strings.NotEnoughMoneyToHunt;
            }

            RemoveMoney(Constants.HuntingCost);
            int skinsGained = new Random().Next(200, 501); // Random skins between 200 and 500
            AddSkins(skinsGained);
            return $"Hunted {skinsGained} skins. {ApplyRandomHuntingEvent()}";
        }

        public string SellToTrader(Trader trader)
        {
            return SellSkins(trader, Constants.DeerSkinPrice);
        }
    }
}
