using DeerskinSimulation.Resources;

namespace DeerskinSimulation.Models
{
    public class RoleExporter : ParticipantRole
    {
        private IRandomEventStrategy _exportingEventStrategy;

        public RoleExporter(string name) : base(name, Constants.ExporterStartingFunds, Constants.ExporterStartingSkins) 
        {
            _exportingEventStrategy = new RandomEventStrategyExporting();
        }

        public EventResult Export(int numberOfSkins)
        {
            if (Skins < numberOfSkins)
            {
                return new EventResult(new EventRecord(Strings.NotEnoughSkinsToExport, "images/merchant_ship_256.jpg"));
            }

            return ExportSkins(numberOfSkins, Constants.TransatlanticTransportCost, Constants.ExportDuty, Constants.DeerSkinPricePerLb, Constants.ExporterMarkup);
        }
        protected EventResult ApplyRandomExportingEvent()
        {
            return ApplyRandomEvent(_exportingEventStrategy);
        }

        public EventResult RollForRandomExportingEvent()
        {
            return ApplyRandomExportingEvent();
        }

        public virtual EventResult ExportSkins(int numberOfSkins, double exportCost, double duty, double pricePerSkin, double markup)
        {
            if (_skins < numberOfSkins)
            {
                return new EventResult(new EventRecord(Strings.NoSkinsToExport));
            }

            double principal = MathUtils.CalculateTransactionCost(numberOfSkins, pricePerSkin);
            double totalCost = MathUtils.CalculateTotalCost(principal, exportCost, duty);
            double sellingPrice = MathUtils.CalculateSellingPrice(principal + totalCost, markup);

            if (_money < totalCost)
            {
                return new EventResult(new EventRecord(Strings.NotEnoughMoneyToExport));
            }

            RemoveMoney(totalCost);
            AddMoney(sellingPrice);
            RemoveSkins(numberOfSkins);

            var eventResult = ApplyRandomExportingEvent();
            eventResult.Records.Add(new EventRecord($"Exported {numberOfSkins} skins."));

            return eventResult;
        }

    }
}
