namespace DeerskinSimulation.Models
{
    public class Trader : Participant
    {
        public Trader(string name) : base(name, Constants.TraderStartingFunds) { }

        public string BuySkins(Hunter hunter)
        {
            return hunter.SellToTrader(this);
        }

        public string TransportToExporter(Exporter exporter)
        {
            return TransportSkins(exporter, Constants.RegionalTransportCost, Constants.DeerSkinPrice, Constants.TraderMarkup);
        }
    }
}
