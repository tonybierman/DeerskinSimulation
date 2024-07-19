namespace DeerskinSimulation.Models
{
    public static class EventManager
    {
        public static string ApplyRandomHuntingEvent(Participant participant)
        {
            // Define context-specific random events for hunting
            var rand = new Random();
            var chance = rand.Next(0, 100);
            if (chance < 10) // 10% chance
            {
                int extraSkins = rand.Next(Constants.ExtraSkinsMin, Constants.ExtraSkinsMax);
                participant.AddSkins(extraSkins);
                return "Found extra skins during the hunt!";
            }
            else if (chance < 20) // 10% chance
            {
                int lostSkins = rand.Next(Constants.LostSkinsMin, Constants.LostSkinsMax);
                participant.RemoveSkins(lostSkins);
                return "Lost some skins due to bad weather.";
            }
            return string.Empty;
        }

        public static string ApplyRandomTradingEvent(Participant participant)
        {
            // Define context-specific random events for trading
            var rand = new Random();
            var chance = rand.Next(0, 100);
            if (chance < 10) // 10% chance
            {
                double bonusMoney = rand.Next((int)Constants.BonusMoneyMin, (int)Constants.BonusMoneyMax);
                participant.AddMoney(bonusMoney);
                return "High demand increased the selling price.";
            }
            else if (chance < 20) // 10% chance
            {
                int damagedSkins = rand.Next(Constants.LostSkinsMin, Constants.LostSkinsMax);
                participant.RemoveSkins(damagedSkins);
                return "Damaged some skins while trading.";
            }
            return string.Empty;
        }

        public static string ApplyRandomTransportingEvent(Participant participant)
        {
            // Define context-specific random events for transporting
            var rand = new Random();
            var chance = rand.Next(0, 100);
            if (chance < 10) // 10% chance
            {
                double bonusMoney = rand.Next((int)Constants.BonusMoneyMin, (int)Constants.BonusMoneyMax);
                participant.AddMoney(bonusMoney);
                return "Found a faster route, saved on costs.";
            }
            else if (chance < 20) // 10% chance
            {
                int lostSkins = rand.Next(Constants.LostSkinsMin, Constants.LostSkinsMax);
                participant.RemoveSkins(lostSkins);
                return "Lost some skins during transport.";
            }
            return string.Empty;
        }

        public static string ApplyRandomExportingEvent(Participant participant)
        {
            // Define context-specific random events for exporting
            var rand = new Random();
            var chance = rand.Next(0, 100);
            if (chance < 10) // 10% chance
            {
                double bonusMoney = rand.Next((int)Constants.BonusMoneyMin, (int)Constants.BonusMoneyMax);
                participant.AddMoney(bonusMoney);
                return "High demand in Europe increased the selling price.";
            }
            else if (chance < 20) // 10% chance
            {
                int lostSkins = rand.Next(Constants.LostSkinsMin, Constants.LostSkinsMax);
                participant.RemoveSkins(lostSkins);
                return "Lost some skins during export.";
            }
            return string.Empty;
        }
    }
}
