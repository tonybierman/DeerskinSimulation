namespace DeerskinSimulation.Models
{
    public class RandomEventStrategyHunting : IRandomEventStrategy
    {
        public EventResult ApplyEvent(ParticipantRole participant)
        {
            var rand = new Random();
            var chance = rand.Next(0, 100);
            if (chance < 3) // 3% chance
            {
                int extraSkins = rand.Next(Constants.ExtraSkinsMin, Constants.ExtraSkinsMax);
                return new RandomEventResultFoundSkins(extraSkins);
            }
            else if (chance < 6) // 3% chance
            {
                if (participant.Skins > 0)
                {
                    int lostSkins = rand.Next(Constants.LostSkinsMin, Constants.LostSkinsMax);
                    return new RandomEventResultLostSkins(lostSkins);
                }
            }

            return new EventResult();
        }
    }
}
