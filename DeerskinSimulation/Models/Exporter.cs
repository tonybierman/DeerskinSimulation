namespace DeerskinSimulation.Models
{
    public class Exporter : Participant
    {
        public Exporter(string name) : base(name, Constants.ExporterStartingFunds) { }

        public override string ExportSkins(double exportCost, double duty, double pricePerSkin, double markup)
        {
            return base.ExportSkins(exportCost, duty, pricePerSkin, markup);
        }

        public string Export()
        {
            return ExportSkins(Constants.TransatlanticTransportCost, Constants.ExportDuty, Constants.DeerSkinPrice, Constants.ExporterMarkup);
        }
    }
}
