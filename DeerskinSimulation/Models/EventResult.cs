using System;
using System.Collections.Generic;

namespace DeerskinSimulation.Models
{
    public delegate void EventAction(Participant participant);

    public class EventResult
    {
        public List<EventRecord> Records { get; }
        public EventAction OriginatorAction { get; }
        public EventAction RecipientAction { get; }

        public EventResult(EventRecord record, EventAction originatorAction = null, EventAction recipientAction = null)
        {
            Records = new List<EventRecord> { record };
            OriginatorAction = originatorAction;
            RecipientAction = recipientAction;
        }

        public void ApplyActions(Participant originator, Participant recipient = null)
        {
            OriginatorAction?.Invoke(originator);
            RecipientAction?.Invoke(recipient);
        }

        public static EventResult Empty => new EventResult(EventRecord.Empty);
    }
}
