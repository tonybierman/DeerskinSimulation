using DeerskinSimulation.Resources;

namespace DeerskinSimulation.Models
{
    public class Exporter : Participant
    {
        public Exporter(string name) : base(name, Constants.ExporterStartingFunds) { }

        public EventResult Export(int numberOfSkins)
        {
            if (Skins < numberOfSkins)
            {
                return new EventResult(new EventRecord(Strings.NotEnoughSkinsToExport));
            }

            return ExportSkins(numberOfSkins, Constants.TransatlanticTransportCost, Constants.ExportDuty, Constants.DeerSkinPrice, Constants.ExporterMarkup);
        }

    }
}
