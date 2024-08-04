using DeerskinSimulation.Resources;
using DeerskinSimulation.ViewModels;

namespace DeerskinSimulation.Models
{
    public class RoleExporter : ParticipantRole
    {
        private IRandomEventStrategy _exportingEventStrategy;

        public RoleExporter(string name) : this(name, 0, 0, new RandomEventStrategyExporting())
        {
        }

        public RoleExporter(string name, double funds, int skins) : this(name, funds, skins, new RandomEventStrategyExporting())
        {
        }

        public RoleExporter(string name, double funds, int skins, IRandomEventStrategy exportingEventStrategy) : base(name, funds, skins)
        {
            _exportingEventStrategy = exportingEventStrategy;
        }

        public virtual EventResult Export(int numberOfSkins)
        {
            if (Skins < numberOfSkins)
            {
                return new EventResult(new EventRecord(Strings.NotEnoughSkinsToExport, "images/merchant_ship_256.jpg"))
                { Status = EventResultStatus.Fail };
            }

            return ExportSkins(numberOfSkins, Constants.TransAtlanticTransportCost, Constants.ExportDuty, Constants.DeerSkinPricePerLb, Constants.ExporterMarkup);
        }

        public virtual EventResult SeaTravel(ISimulationViewModel viewModel)
        {
            if (viewModel.CurrentUserActivity?.Meta == null)
                throw new NullReferenceException(nameof(TimelapseActivityMeta));

            double netCostPerDay = Constants.SeaTravelCostPerDay;
            TimelapseActivityMeta meta = viewModel.CurrentUserActivity.Meta;

            if (!HasMoney(netCostPerDay))
            {
                return new EventResult(
                    new EventRecord(Strings.NotEnoughMoneyTravel, image: "images/avatar_wm_256.jpg"))
                { Status = EventResultStatus.Fail };
            }

            // Deduct cost
            if (RemoveMoney(netCostPerDay))
            {
                // Create success event
                var eventMessage = new EventResult { Status = EventResultStatus.Success };
                eventMessage.Records.Add(new EventRecord(meta.Name, viewModel.GameDay, $"Sailed about 20 miles.", image: "images/avatar_wm_256.jpg"));
                return eventMessage;
            }

            return new EventResult(new EventRecord("Transaction failed during execution. Money could not be deducted.", "red"))
            {
                Status = EventResultStatus.Fail
            };
        }

        protected EventResult ApplyRandomExportingEvent()
        {
            return ApplyRandomEvent(_exportingEventStrategy);
        }

        public virtual EventResult RollForRandomExportingEvent()
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
