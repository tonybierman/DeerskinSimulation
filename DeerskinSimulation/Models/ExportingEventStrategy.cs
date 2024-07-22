﻿namespace DeerskinSimulation.Models
{
    public class ExportingEventStrategy : IRandomEventStrategy
    {
        public EventRecord ApplyEvent(Participant participant)
        {
            var rand = new Random();
            var chance = rand.Next(0, 100);
            if (chance < 10) // 10% chance
            {
                double bonusMoney = rand.Next((int)Constants.BonusMoneyMin, (int)Constants.BonusMoneyMax);
                participant.AddMoney(bonusMoney);
                return new EventRecord("High demand in Europe increased the selling price.");
            }
            else if (chance < 20) // 10% chance
            {
                int lostSkins = rand.Next(Constants.LostSkinsMin, Constants.LostSkinsMax);
                participant.RemoveSkins(lostSkins);
                return new EventRecord("Lost some skins during export.");
            }

            return EventRecord.Empty;
        }
    }
}
