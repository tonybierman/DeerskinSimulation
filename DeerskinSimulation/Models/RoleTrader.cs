using DeerskinSimulation.Resources;

namespace DeerskinSimulation.Models
{
    public class RoleTrader : ParticipantRole
    {
        private IRandomEventStrategy _transportingEventStrategy;

        public RoleTrader(string name) : base(name, Constants.TraderStartingFunds) 
        {
            _transportingEventStrategy = new RandomEventStrategyTransporting();
        }

        public EventResult TransportToExporter(RoleExporter exporter, int numberOfSkins)
        {
            return TransportSkins(exporter, numberOfSkins, Constants.RegionalTransportCost, Constants.DeerSkinPricePerLb, Constants.TraderMarkup);
        }

        public virtual EventResult TransportSkins(ParticipantRole recipient, int numberOfSkins, double transportCost, double pricePerSkin, double markup)
        {
            if (_skins < numberOfSkins)
            {
                return new EventResult(new EventRecord(Strings.NotEnoughSkinsToTransport));
            }

            double principal = MathUtils.CalculateTransactionCost(numberOfSkins, pricePerSkin);
            double totalCost = principal + transportCost;
            double sellingPrice = MathUtils.CalculateSellingPrice(totalCost, markup);

            if (recipient.Money < sellingPrice)
            {
                return new EventResult(new EventRecord(Strings.RecipientCannotAffordSkins));
            }

            recipient.RemoveMoney(sellingPrice);
            recipient.AddSkins(numberOfSkins);
            RemoveSkins(numberOfSkins);
            AddMoney(sellingPrice);

            var eventResult = ApplyRandomTransportingEvent();
            eventResult.Records.Add(new EventRecord($"Transported {numberOfSkins} skins."));

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
