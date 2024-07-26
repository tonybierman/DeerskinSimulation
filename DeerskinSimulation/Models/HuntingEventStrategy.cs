namespace DeerskinSimulation.Models
{
    public class HuntingEventStrategy : IRandomEventStrategy
    {
        public EventResult ApplyEvent(Participant participant)
        {
            var rand = new Random();
            var chance = rand.Next(0, 100);
            if (chance < 3) // 10% chance
            {
                int extraSkins = rand.Next(Constants.ExtraSkinsMin, Constants.ExtraSkinsMax);
                return new RandomEventFoundSkins(extraSkins);
            }
            else if (chance < 6) // 10% chance
            {
                if (participant.Skins > 0)
                {
                    int lostSkins = rand.Next(Constants.LostSkinsMin, Constants.LostSkinsMax);
                    return new RandomEventLostSkins(lostSkins);
                }
            }

            return new EventResult();
        }
    }
}
