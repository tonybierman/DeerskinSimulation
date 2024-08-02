using DeerskinSimulation.Resources;
using DeerskinSimulation.ViewModels;

namespace DeerskinSimulation.Models
{
    public class RoleTrader : ParticipantRole
    {
        private readonly IRandomEventStrategy _transportingEventStrategy;

        // Default constructor for production
        public RoleTrader(string name) : this(name, 0, 0, new RandomEventStrategyTransporting()) { }

        // Constructor with dependency injection for testing
        public RoleTrader(string name, double funds, int skins, IRandomEventStrategy transportingStrategy = null)
            : base(name, funds, skins)
        {
            _transportingEventStrategy = transportingStrategy ?? new RandomEventStrategyTransporting();
        }

        public EventResult DeliverToExporter(ISimulationViewModel viewModel, RoleExporter exporter, int numberOfSkins)
        {
            var sellingPrice = MathUtils.CalculateTransactionCost(numberOfSkins,
                Constants.DeerSkinPricePerLb * Constants.DeerSkinWeightInLb * Constants.TraderMarkup);

            exporter.RemoveMoney(sellingPrice);
            exporter.AddSkins(numberOfSkins);
            RemoveSkins(numberOfSkins);
            AddMoney(sellingPrice);

            var eventResult = new EventResult();
            eventResult.Records.Add(new EventRecord($"Delivered {numberOfSkins} skins to exporter."));

            return eventResult;
        }

        public virtual EventResult TransportSkins(ISimulationViewModel viewModel, ParticipantRole recipient, int numberOfSkins)
        {
            if (viewModel.CurrentUserActivity?.Meta == null)
                throw new NullReferenceException(nameof(TimelapseActivityMeta));

            double netCostPerDay = Constants.RegionalTransportCost * numberOfSkins;
            TimelapseActivityMeta meta = viewModel.CurrentUserActivity.Meta;

            if (Money < netCostPerDay)
            {
                return new EventResult(
                    new EventRecord(Strings.NotEnoughMoneyTravel, image: "images/avatar_wm_256.jpg"))
                { Status = EventResultStatus.Fail };
            }

            // Deduct cost
            RemoveMoney(netCostPerDay);

            if (_skins < numberOfSkins)
            {
                return new EventResult(new EventRecord(Strings.NotEnoughSkinsToTransport)) { Status = EventResultStatus.Fail };
            }

            var eventResult = new EventResult { Status = EventResultStatus.Success };
            eventResult.Records.Add(new EventRecord(meta.Name, viewModel.GameDay, $"Transported {numberOfSkins} about 20 miles."));

            return eventResult;
        }

        protected EventResult ApplyRandomTransportingEvent()
        {
            return ApplyRandomEvent(_transportingEventStrategy);
        }

        public EventResult RollForRandomTransportingEvent()
        {
            return ApplyRandomTransportingEvent();
        }
    }
}
