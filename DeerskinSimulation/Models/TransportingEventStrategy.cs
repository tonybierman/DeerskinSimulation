namespace DeerskinSimulation.Models
{
    public class TransportingEventStrategy : IRandomEventStrategy
    {
        public string ApplyEvent(Participant participant)
        {
            var rand = new Random();
            var chance = rand.Next(0, 100);
            if (chance < 10) // 10% chance
            {
                double bonusMoney = rand.Next((int)Constants.BonusMoneyMin, (int)Constants.BonusMoneyMax);
                participant.AddMoney(bonusMoney);
                return "Found a faster route, saved on costs.";
            }
            else if (chance < 20) // 10% chance
            {
                int lostSkins = rand.Next(Constants.LostSkinsMin, Constants.LostSkinsMax);
                participant.RemoveSkins(lostSkins);
                return "Lost some skins during transport.";
            }
            return string.Empty;
        }
    }
}
