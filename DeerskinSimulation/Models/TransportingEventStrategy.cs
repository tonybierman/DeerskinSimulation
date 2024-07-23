﻿namespace DeerskinSimulation.Models
{
    public class TransportingEventStrategy : IRandomEventStrategy
    {
        public EventResult ApplyEvent(Participant participant)
        {
            var rand = new Random();
            var chance = rand.Next(0, 100);
            if (chance < 10) // 10% chance
            {
                double bonusMoney = rand.Next((int)Constants.BonusMoneyMin, (int)Constants.BonusMoneyMax);
                return new EventResult(
                    new EventRecord("Found a faster route, saved on costs.", "green"),
                    originator => originator.AddMoney(bonusMoney),
                    recipient => recipient?.RemoveMoney(bonusMoney)
                );
            }
            else if (chance < 20) // 10% chance
            {
                int lostSkins = rand.Next(Constants.LostSkinsMin, Constants.LostSkinsMax);
                return new EventResult(
                    new EventRecord("Lost some skins during transport.", "red"),
                    originator => originator.RemoveSkins(lostSkins),
                    recipient => recipient?.AddSkins(lostSkins)
                );
            }
            return EventResult.Empty;
        }
    }
}
