using DeerskinSimulation.Resources;
using DeerskinSimulation.ViewModels;

namespace DeerskinSimulation.Models
{
    public class RoleTrader : ParticipantRole
    {
        private IRandomEventStrategy _transportingEventStrategy;

        public RoleTrader(string name) : base(name, Constants.TraderStartingFunds) 
        {
            _transportingEventStrategy = new RandomEventStrategyTransporting();
        }

        public EventResult DeliverToExporter(SimulationViewModel viewModel, RoleExporter exporter, int numberOfSkins)
        {
            var sellingPrice = MathUtils.CalculateTransactionCost(numberOfSkins, Constants.DeerSkinPricePerLb * Constants.DeerSkinWeightInLb);

            exporter.RemoveMoney(sellingPrice);
            exporter.AddSkins(numberOfSkins);
            RemoveSkins(numberOfSkins);
            AddMoney(sellingPrice);

            var eventResult = new EventResult();
            eventResult.Records.Add(new EventRecord($"Delivered {numberOfSkins} skins to exporter."));

            return eventResult;
        }

        public virtual EventResult TransportSkins(SimulationViewModel viewModel, ParticipantRole recipient, int numberOfSkins)
        {
            if (viewModel.CurrentUserActivity?.Meta == null)
                throw new NullReferenceException(nameof(TimelapseActivityMeta));

            double netCostPerDay = Constants.RegionalTransportCost * numberOfSkins;
            TimelapseActivityMeta meta = viewModel.CurrentUserActivity.Meta;

            if (Money < netCostPerDay)
            {
                return new EventResult(
                    new EventRecord(Strings.NotEnoughMoneyTravel,
                        image: "images/avatar_wm_256.jpg"))
                { Status = EventResultStatus.Fail };
            }

            // Gotta pay to play
            RemoveMoney(netCostPerDay);

            if (_skins < numberOfSkins)
            {
                return new EventResult(new EventRecord(Strings.NotEnoughSkinsToTransport)) { Status = EventResultStatus.Fail };
            }

            var eventResult = new EventResult();
            eventResult.Records.Add(new EventRecord($"Transported {numberOfSkins} skins about 20 miles."));

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
