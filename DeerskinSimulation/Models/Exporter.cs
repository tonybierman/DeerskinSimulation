using DeerskinSimulation.Resources;

namespace DeerskinSimulation.Models
{
    public class Exporter : Participant
    {
        public Exporter(string name) : base(name, Constants.ExporterStartingFunds) { }

        public string Export(int numberOfSkins)
        {
            if (Skins < numberOfSkins)
            {
                return Strings.NotEnoughSkinsToExport;
            }
            return ExportSkins(numberOfSkins, Constants.TransatlanticTransportCost, Constants.ExportDuty, Constants.DeerSkinPrice, Constants.ExporterMarkup);
        }

    }
}
