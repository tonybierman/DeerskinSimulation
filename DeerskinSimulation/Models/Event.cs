namespace DeerskinSimulation.Models
{
    using System;
    using System.Resources;

    public abstract class Event
    {
        public abstract string Apply(Participant participant);
    }

    public abstract class Fortune : Event
    {
        protected readonly string Description;
        protected readonly Action<Participant> Effect;

        protected Fortune(string description, Action<Participant> effect)
        {
            Description = description;
            Effect = effect;
        }

        public override string Apply(Participant participant)
        {
            Effect(participant);
            return Description;
        }
    }

    public abstract class Misfortune : Event
    {
        protected readonly string Description;
        protected readonly Action<Participant> Effect;

        protected Misfortune(string description, Action<Participant> effect)
        {
            Description = description;
            Effect = effect;
        }

        public override string Apply(Participant participant)
        {
            Effect(participant);
            return Description;
        }
    }

    public class HuntingFortune : Fortune
    {
        public HuntingFortune(string description, Action<Participant> effect)
            : base(description, effect) { }
    }

    public class HuntingMisfortune : Misfortune
    {
        public HuntingMisfortune(string description, Action<Participant> effect)
            : base(description, effect) { }
    }

    public class TradingFortune : Fortune
    {
        public TradingFortune(string description, Action<Participant> effect)
            : base(description, effect) { }
    }

    public class TradingMisfortune : Misfortune
    {
        public TradingMisfortune(string description, Action<Participant> effect)
            : base(description, effect) { }
    }

    public class TransportingFortune : Fortune
    {
        public TransportingFortune(string description, Action<Participant> effect)
            : base(description, effect) { }
    }

    public class TransportingMisfortune : Misfortune
    {
        public TransportingMisfortune(string description, Action<Participant> effect)
            : base(description, effect) { }
    }

    public class ExportingFortune : Fortune
    {
        public ExportingFortune(string description, Action<Participant> effect)
            : base(description, effect) { }
    }

    public class ExportingMisfortune : Misfortune
    {
        public ExportingMisfortune(string description, Action<Participant> effect)
            : base(description, effect) { }
    }


}
