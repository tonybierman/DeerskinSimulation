namespace DeerskinSimulation.Models
{
    public class RoleTrader : ParticipantRole
    {
        public RoleTrader(string name) : base(name, Constants.TraderStartingFunds) { }

        public EventResult TransportToExporter(RoleExporter exporter, int numberOfSkins)
        {
            return TransportSkins(exporter, numberOfSkins, Constants.RegionalTransportCost, Constants.DeerSkinPrice, Constants.TraderMarkup);
        }

        public EventResult RollForRandomTransportingEvent()
        {
            return ApplyRandomTransportingEvent();
        }
    }
}
