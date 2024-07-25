namespace DeerskinSimulation.Models
{
    public class Trader : Participant
    {
        public Trader(string name) : base(name, Constants.TraderStartingFunds) { }

        public EventResult BuySkins(Hunter hunter, int numberOfSkins)
        {
            return hunter.SellToTrader(this, numberOfSkins);
        }

        public EventResult TransportToExporter(Exporter exporter, int numberOfSkins)
        {
            return TransportSkins(exporter, numberOfSkins, Constants.RegionalTransportCost, Constants.DeerSkinPrice, Constants.TraderMarkup);
        }

        public EventResult RollForRandomForwardingEvent()
        {
            return ApplyRandomForwardingEvent();
        }
    }
}
