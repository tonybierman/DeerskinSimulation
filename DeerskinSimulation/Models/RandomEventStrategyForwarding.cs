namespace DeerskinSimulation.Models
{
    public class RandomEventStrategyForwarding : IRandomEventStrategy
    {
        public EventResult ApplyEvent(ParticipantRole participant)
        {
            var rand = new Random();
            var chance = rand.Next(0, 100);

            if (chance < 3) // 3% chance
            {
                double bonusMoney = rand.Next((int)Constants.BonusMoneyMin, (int)Constants.BonusMoneyMax);
                return new RandomEventResultHighDemand(bonusMoney);
            }
            else if (chance < 6) // 3% chance
            {
                int damagedSkins = rand.Next(Constants.LostSkinsMin, Constants.LostSkinsMax);
                return new RandomEventResultDamagedSkins(damagedSkins);
            }

            return new EventResult();
        }
    }
}
