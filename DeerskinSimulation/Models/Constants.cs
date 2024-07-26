namespace DeerskinSimulation.Models
{
    public static class Constants
    {
        // Starting funds
        public const double HunterStartingFunds = 100.0;
        public const double TraderStartingFunds = 1000.0;
        public const double ExporterStartingFunds = 10000.0;
        
        // Sundry
        public const double DeerSkinPricePerLb = 0.4;
        public const double DeerSkinWeightInLb = 2.5;
        public const double RegionalTransportCost = 0.1;
        public const double TransatlanticTransportCost = 0.5;
        public const double ExportDuty = 0.2;
        public const double HuntingCostPerDay = 1.0;
        public const double TraderMarkup = 0.3;
        public const double ExporterMarkup = 0.5;
        public const int MinimumSkinsToTransport = 100;

        // Skins
        public const int DailySkinsMin = 1;
        public const int DailySkinsMax = 10;
        public const int ExtraSkinsMin = 1;
        public const int ExtraSkinsMax = 5;
        public const int LostSkinsMin = 1;
        public const int LostSkinsMax = 5;

        // Packhorses
        public const int PackhorseCapacity = 100;
        public const double PackhorseCost = 100.0;
        public const int MaxPackhorses = 5;

        // Bonus money constants
        public const double BonusMoneyMin = 20.0;
        public const double BonusMoneyMax = 100.0;
    }
}
