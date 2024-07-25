namespace DeerskinSimulation.Models
{
    public class Trader : Participant
    {
        public Trader(string name) : base(name, Constants.TraderStartingFunds) { }

        public EventResult TransportToExporter(Exporter exporter, int numberOfSkins)
        {
            return TransportSkins(exporter, numberOfSkins, Constants.RegionalTransportCost, Constants.DeerSkinPrice, Constants.TraderMarkup);
        }

        public EventResult RollForRandomTransportingEvent()
        {
            return ApplyRandomTransportingEvent();
        }
    }
}
