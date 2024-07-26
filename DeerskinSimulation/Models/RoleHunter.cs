namespace DeerskinSimulation.Models
{
    using System;
    using DeerskinSimulation.Resources; // Make sure you have the appropriate using directive

    public class RoleHunter : ParticipantRole
    {
        private IRandomEventStrategy _huntingEventStrategy;
        private IRandomEventStrategy _forwardingEventStrategy;

        public RoleHunter(string name) : base(name, Constants.HunterStartingFunds) 
        {
            _huntingEventStrategy = new RandomEventStrategyHunting();
            _forwardingEventStrategy = new RandomEventStrategyForwarding();
        }

        /// <summary>
        /// Hunting math
        /// It costs money to hunt everyday
        /// </summary>
        /// <param name="packhorses"></param>
        /// <returns></returns>
        public EventResult Hunt(int packhorses)
        {
            if (Money < Constants.HuntingCostPerDay)
            {
                return new EventResult(new EventRecord(Strings.NotEnoughMoneyToHunt, image: "images/avatar_wm_256.jpg"));
            }

            // Gotta pay to play
            RemoveMoney(Constants.HuntingCostPerDay * packhorses);
            
            // Now try to get some skins
            var rand = new Random();
            var skinsHunted = rand.Next(Constants.DailySkinsMin + packhorses, Constants.DailySkinsMax + packhorses);
            AddSkins(skinsHunted);

            // Tell the UI
            var eventMessage = new EventResult();
            eventMessage.Records.Add(new EventRecord($"{Strings.HuntedSkins} {skinsHunted}.", image: "images/avatar_wm_256.jpg"));

            return eventMessage;
        }

        public EventResult ForwardToTrader(RoleTrader trader, int numberOfSkins)
        {
            if (Skins < numberOfSkins)
            {
                return new EventResult(new EventRecord(Strings.NotEnoughSkinsToSell, image: "images/avatar_wm_256.jpg"));
            }

            double totalCost = numberOfSkins * Constants.DeerSkinPricePerLb * Constants.DeerSkinWeightInLb;
            trader.RemoveMoney(totalCost);
            trader.AddSkins(numberOfSkins);

            RemoveSkins(numberOfSkins);
            AddMoney(totalCost);

            var eventMessage = new EventResult();
            eventMessage.Records.Add(new EventRecord($"{Strings.ForwardedSkins} {numberOfSkins}.", image: "images/avatar_wm_256.jpg"));

            return eventMessage;
        }

        protected EventResult ApplyRandomHuntingEvent()
        {
            return ApplyRandomEvent(_huntingEventStrategy);
        }
        protected EventResult ApplyRandomForwardingEvent()
        {
            return ApplyRandomEvent(_forwardingEventStrategy);
        }

        public EventResult RollForRandomHuntingEvent()
        {
            return ApplyRandomHuntingEvent();
        }

        public EventResult RollForRandomForwardingEvent()
        {
            return ApplyRandomForwardingEvent();
        }
    }
}
