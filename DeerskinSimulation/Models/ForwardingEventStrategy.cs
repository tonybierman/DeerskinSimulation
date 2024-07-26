namespace DeerskinSimulation.Models
{
    public class ForwardingEventStrategy : IRandomEventStrategy
    {
        public EventResult ApplyEvent(Participant participant)
        {
            var rand = new Random();
            var chance = rand.Next(0, 100);

            if (chance < 3) // 10% chance
            {
                double bonusMoney = rand.Next((int)Constants.BonusMoneyMin, (int)Constants.BonusMoneyMax);
                return new EventResult(
                    new EventRecord("High demand increased the selling price.", "green", "images/packhorse_256.jpg"),
                    originator => originator.AddMoney(bonusMoney),
                    recipient => recipient?.RemoveMoney(bonusMoney)
                );
            }
            else if (chance < 6) // 10% chance
            {
                int damagedSkins = rand.Next(Constants.LostSkinsMin, Constants.LostSkinsMax);
                damagedSkins = 5;
                return new EventResult(
                    new EventRecord("Damaged some skins while forwarding.", "red", "images/packhorse_256.jpg"),
                    originator => originator.RemoveSkins(damagedSkins)
                );
            }

            return new EventResult();
        }
    }
}
