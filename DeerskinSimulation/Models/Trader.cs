namespace DeerskinSimulation.Models
{
    public class Trader : Participant
    {
        public Trader(string name) : base(name, Constants.TraderStartingFunds) { }

        public EventRecord BuySkins(Hunter hunter, int numberOfSkins)
        {
            return hunter.SellToTrader(this, numberOfSkins);
        }

        public EventRecord TransportToExporter(Exporter exporter, int numberOfSkins)
        {
            return TransportSkins(exporter, numberOfSkins, Constants.RegionalTransportCost, Constants.DeerSkinPrice, Constants.TraderMarkup);
        }
    }
}
