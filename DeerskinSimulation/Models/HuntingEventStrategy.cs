namespace DeerskinSimulation.Models
{
    public class HuntingEventStrategy : IRandomEventStrategy
    {
        public EventResult ApplyEvent(Participant participant)
        {
            var rand = new Random();
            var chance = rand.Next(0, 100);
            if (chance < 2) // 10% chance
            {
                int extraSkins = rand.Next(Constants.ExtraSkinsMin, Constants.ExtraSkinsMax);
                return new EventResult(
                    new EventRecord("Found extra skins during the hunt!", "green", "images/good_fortune_256.jpg"),
                    p => p.AddSkins(extraSkins)
                );
            }
            else if (chance < 4) // 10% chance
            {
                int lostSkins = rand.Next(Constants.LostSkinsMin, Constants.LostSkinsMax);
                return new EventResult(
                    new EventRecord("Lost some skins due to bad weather.", "red", "images/bad_fortune_256.jpg"),
                    p => p.RemoveSkins(lostSkins)
                );
            }

            return EventResult.Empty;
        }
    }
}
