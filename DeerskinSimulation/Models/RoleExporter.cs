using DeerskinSimulation.Resources;

namespace DeerskinSimulation.Models
{
    public class RoleExporter : ParticipantRole
    {
        private IRandomEventStrategy _exportingEventStrategy;

        public RoleExporter(string name) : this(name, 0, 0) 
        {
        }

        public RoleExporter(string name, double funds, int skins) : base(name, funds, skins)
        {
            _exportingEventStrategy = new RandomEventStrategyTransporting();
        }

        public EventResult Export(int numberOfSkins)
        {
            if (Skins < numberOfSkins)
            {
                return new EventResult(new EventRecord(Strings.NotEnoughSkinsToExport, "images/merchant_ship_256.jpg")) { Status = EventResultStatus.Fail };
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
                return new EventResult(new EventRecord(Strings.NoSkinsToExport)) { Status = EventResultStatus.Fail };
            }

            double principal = MathUtils.CalculateTransactionCost(numberOfSkins, pricePerSkin);
            double totalCost = MathUtils.CalculateTotalCost(principal, exportCost, duty);
            double sellingPrice = MathUtils.CalculateSellingPrice(principal + totalCost, markup);

            if (_money < totalCost)
            {
                return new EventResult(new EventRecord(Strings.NotEnoughMoneyToExport)) { Status = EventResultStatus.Fail };
            }

            RemoveMoney(totalCost);
            AddMoney(sellingPrice);
            RemoveSkins(numberOfSkins);

            var eventResult = ApplyRandomExportingEvent();
            if (eventResult.Status == EventResultStatus.None)
            {
                eventResult.Status = EventResultStatus.Success;
            }
            eventResult.Records.Add(new EventRecord($"Exported {numberOfSkins} skins."));

            return eventResult;
        }

    }
}
