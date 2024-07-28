﻿namespace DeerskinSimulation.Models
{
    using System;
    using DeerskinSimulation.Pages;
    using DeerskinSimulation.Resources; // Make sure you have the appropriate using directive
    using DeerskinSimulation.ViewModels;

    public class RoleHunter : ParticipantRole
    {
        private IRandomEventStrategy _huntingEventStrategy;
        private IRandomEventStrategy _forwardingEventStrategy;

        public RoleHunter(string name) : base(name, Constants.HunterStartingFunds) 
        {
            _huntingEventStrategy = new RandomEventStrategyHunting();
            _forwardingEventStrategy = new RandomEventStrategyForwarding();
        }

        public EventResult Travel(SimulationViewModel viewModel)
        {
            double netCostPerDay = Constants.HuntingCostPerDay * viewModel.SelectedPackhorses;

            if (Money < netCostPerDay)
            {
                return new EventResult(
                    new EventRecord(Strings.NotEnoughMoneyToHunt,
                        image: "images/avatar_wm_256.jpg"))
                { Status = EventResultStatus.Fail };
            }

            // Gotta pay to play
            RemoveMoney(netCostPerDay);

            // Tell the UI
            var eventMessage = new EventResult();
            eventMessage.Records.Add(new EventRecord($"Traveled about 20 miles.", image: "images/avatar_wm_256.jpg"));

            return eventMessage;
        }

        /// <summary>
        /// Hunting math
        /// It costs money to hunt everyday
        /// </summary>
        /// <param name="packhorses"></param>
        /// <returns></returns>
        public EventResult Hunt(SimulationViewModel viewModel)
        {
            if (viewModel.CurrentUserActivity?.Meta == null)
                throw new NullReferenceException(nameof(TimelapseActivityMeta));

            double netCostPerDay = Constants.HuntingCostPerDay * viewModel.SelectedPackhorses;
            TimelapseActivityMeta meta = viewModel.CurrentUserActivity.Meta;

            NullReferenceException? exception = null;
            if (Money < netCostPerDay)
            {
                return new EventResult(
                    new EventRecord(Strings.NotEnoughMoneyToHunt, 
                        image: "images/avatar_wm_256.jpg")) 
                    { Status = EventResultStatus.Fail } ;
            }

            // Gotta pay to play
            RemoveMoney(netCostPerDay);
            
            // Now try to get some skins
            var rand = new Random();
            var skinsHunted = rand.Next(Constants.DailySkinsMin + viewModel.SelectedPackhorses, Constants.DailySkinsMax + viewModel.SelectedPackhorses);
            AddSkins(skinsHunted);

            // Tell the UI
            var eventMessage = new EventResult();
            eventMessage.Records.Add(new EventRecord(meta.Name, meta.Elapsed, string.Format(Strings.HuntedSkins, skinsHunted), image: "images/avatar_wm_256.jpg"));

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
