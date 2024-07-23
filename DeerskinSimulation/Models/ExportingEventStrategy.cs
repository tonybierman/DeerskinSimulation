namespace DeerskinSimulation.Models
{
    public class ExportingEventStrategy : IRandomEventStrategy
    {
        public EventResult ApplyEvent(Participant participant)
        {
            var rand = new Random();
            var chance = rand.Next(0, 100);

            if (chance < 10) // 10% chance
            {
                double bonusMoney = rand.Next((int)Constants.BonusMoneyMin, (int)Constants.BonusMoneyMax);
                return new EventResult(
                    new EventRecord("High demand in Europe increased the selling price.", "green", "images/merchant_ship_256.jpg"),
                    originator => originator.AddMoney(bonusMoney),
                    recipient => recipient?.RemoveMoney(bonusMoney)
                );
            }
            else if (chance < 20) // 10% chance
            {
                int lostSkins = rand.Next(Constants.LostSkinsMin, Constants.LostSkinsMax);
                return new EventResult(
                    new EventRecord("Lost some skins during export.", "red", "images/merchant_ship_256.jpg"),
                    originator => originator.RemoveSkins(lostSkins),
                    recipient => recipient?.AddSkins(lostSkins)
                );
            }

            return EventResult.Empty;
        }
    }
}
