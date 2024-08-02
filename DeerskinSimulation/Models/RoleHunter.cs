namespace DeerskinSimulation.Models
{
    using System;
    using DeerskinSimulation.Pages;
    using DeerskinSimulation.Resources;
    using DeerskinSimulation.ViewModels;

    public class RoleHunter : ParticipantRole
    {
        private IRandomEventStrategy _huntingEventStrategy;
        private IRandomEventStrategy _forwardingEventStrategy;

        // Default constructor for production
        public RoleHunter(string name) : this(name, 0, 0, new RandomEventStrategyHunting(), new RandomEventStrategyForwarding()) { }

        // Constructor with dependency injection for testing
        public RoleHunter(string name, double funds, int skins,
                          IRandomEventStrategy huntingStrategy = null,
                          IRandomEventStrategy forwardingStrategy = null) : base(name, funds, skins)
        {
            _huntingEventStrategy = huntingStrategy ?? new RandomEventStrategyHunting();
            _forwardingEventStrategy = forwardingStrategy ?? new RandomEventStrategyForwarding();
        }

        public EventResult Travel(ISimulationViewModel viewModel)
        {
            if (viewModel.CurrentUserActivity?.Meta == null)
                throw new NullReferenceException(nameof(TimelapseActivityMeta));

            double netCostPerDay = Constants.HuntingCostPerDay * viewModel.SelectedPackhorses;
            TimelapseActivityMeta meta = viewModel.CurrentUserActivity.Meta;

            if (Money < netCostPerDay)
            {
                return new EventResult(
                    new EventRecord(Strings.NotEnoughMoneyToHunt, image: "images/avatar_wm_256.jpg"))
                { Status = EventResultStatus.Fail };
            }

            // Deduct cost
            RemoveMoney(netCostPerDay);

            // Create success event
            var eventMessage = new EventResult { Status = EventResultStatus.Success };
            eventMessage.Records.Add(new EventRecord(meta.Name, viewModel.GameDay, $"Traveled about 20 miles.", image: "images/avatar_wm_256.jpg"));

            return eventMessage;
        }

        public virtual EventResult Hunt(ISimulationViewModel viewModel)
        {
            if (viewModel.CurrentUserActivity?.Meta == null)
                throw new NullReferenceException(nameof(TimelapseActivityMeta));

            double netCostPerDay = Constants.HuntingCostPerDay * viewModel.SelectedPackhorses;
            TimelapseActivityMeta meta = viewModel.CurrentUserActivity.Meta;

            if (Money < netCostPerDay)
            {
                return new EventResult(
                    new EventRecord(Strings.NotEnoughMoneyToHunt, image: "images/avatar_wm_256.jpg"))
                { Status = EventResultStatus.Fail };
            }

            // Deduct cost
            RemoveMoney(netCostPerDay);

            // Determine skins hunted
            var rand = new Random();
            var skinsHunted = rand.Next(Constants.DailySkinsMin + viewModel.SelectedPackhorses, Constants.DailySkinsMax + viewModel.SelectedPackhorses);
            AddSkins(skinsHunted);

            // Create success event
            var eventMessage = new EventResult { Status = EventResultStatus.Success };
            eventMessage.Records.Add(new EventRecord(meta.Name, viewModel.GameDay, string.Format(Strings.HuntedSkins, skinsHunted), image: "images/avatar_wm_256.jpg"));

            CurrentBag += skinsHunted;

            return eventMessage;
        }

        public EventResult EndHunt(ISimulationViewModel viewModel)
        {
            if (viewModel.CurrentUserActivity?.Meta == null)
                throw new NullReferenceException(nameof(TimelapseActivityMeta));

            TimelapseActivityMeta meta = viewModel.CurrentUserActivity.Meta;

            var eventMessage = new EventResult();
            string msg = string.Format(Strings.EndOfHunt, CurrentBag, meta.Duration);
            eventMessage.Records.Add(new EventRecord(meta.Name, viewModel.GameDay, msg, image: "images/avatar_wm_256.jpg"));

            return eventMessage;
        }

        public EventResult DeliverToTrader(RoleTrader trader, int numberOfSkins)
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
            eventMessage.Records.Add(new EventRecord(string.Format(Strings.ForwardedSkins, numberOfSkins, trader.Name), image: "images/avatar_wm_256.jpg"));

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
