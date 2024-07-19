namespace DeerskinSimulation.Models
{
    public static class Constants
    {
        public const double HunterStartingFunds = 500.0;
        public const double TraderStartingFunds = 5000.0;
        public const double ExporterStartingFunds = 10000.0;
        public const double DeerSkinPrice = 0.4;
        public const double RegionalTransportCost = 0.1;
        public const double TransatlanticTransportCost = 0.5;
        public const double ExportDuty = 0.2;
        public const double HuntingCost = 100.0;
        public const double TraderMarkup = 0.3;
        public const double ExporterMarkup = 0.5;
        public const int MinimumSkinsToTransport = 100;
        public const int ExtraSkinsMin = 10;
        public const int ExtraSkinsMax = 50;
        public const int LostSkinsMin = 10;
        public const int LostSkinsMax = 50;

        // New constants
        public const int PackhorseCapacity = 100;
        public const double PackhorseCost = 100.0;
        public const int MaxPackhorses = 5;

        // Bonus money constants
        public const double BonusMoneyMin = 20.0;
        public const double BonusMoneyMax = 100.0;
    }
}
