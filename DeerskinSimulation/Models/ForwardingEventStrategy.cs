namespace DeerskinSimulation.Models
{
    public class ForwardingEventStrategy : IRandomEventStrategy
    {
        public EventResult ApplyEvent(Participant participant)
        {
            var rand = new Random();
            var chance = rand.Next(0, 100);

            if (chance < 3) // 3% chance
            {
                double bonusMoney = rand.Next((int)Constants.BonusMoneyMin, (int)Constants.BonusMoneyMax);
                return new RandomEventHighDemand(bonusMoney);
            }
            else if (chance < 6) // 3% chance
            {
                int damagedSkins = rand.Next(Constants.LostSkinsMin, Constants.LostSkinsMax);
                return new RandomEventDamagedSkins(damagedSkins);
            }

            return new EventResult();
        }
    }
}
