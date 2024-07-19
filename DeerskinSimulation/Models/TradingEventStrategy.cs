namespace DeerskinSimulation.Models
{
    public class TradingEventStrategy : IRandomEventStrategy
    {
        public string ApplyEvent(Participant participant)
        {
            var rand = new Random();
            var chance = rand.Next(0, 100);
            if (chance < 10) // 10% chance
            {
                double bonusMoney = rand.Next((int)Constants.BonusMoneyMin, (int)Constants.BonusMoneyMax);
                participant.AddMoney(bonusMoney);
                return "High demand increased the selling price.";
            }
            else if (chance < 20) // 10% chance
            {
                int damagedSkins = rand.Next(Constants.LostSkinsMin, Constants.LostSkinsMax);
                participant.RemoveSkins(damagedSkins);
                return "Damaged some skins while trading.";
            }
            return string.Empty;
        }
    }
}
