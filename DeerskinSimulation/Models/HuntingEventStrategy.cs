namespace DeerskinSimulation.Models
{
    public class HuntingEventStrategy : IRandomEventStrategy
    {
        public EventRecord ApplyEvent(Participant participant)
        {
            var rand = new Random();
            var chance = rand.Next(0, 100);
            if (chance < 10) // 10% chance
            {
                int extraSkins = rand.Next(Constants.ExtraSkinsMin, Constants.ExtraSkinsMax);
                participant.AddSkins(extraSkins);
                return new EventRecord("Found extra skins during the hunt!");
            }
            else if (chance < 20) // 10% chance
            {
                int lostSkins = rand.Next(Constants.LostSkinsMin, Constants.LostSkinsMax);
                participant.RemoveSkins(lostSkins);
                return new EventRecord("Lost some skins due to bad weather.");
            }
            return EventRecord.Empty;
        }
    }
}
