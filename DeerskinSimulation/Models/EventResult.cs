using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;

namespace DeerskinSimulation.Models
{
    /// <summary>
    /// Delegate for defining actions that can be performed on a participant.
    /// </summary>
    /// <param name="participant">The participant on which the action is performed.</param>
    public delegate void EventAction(ParticipantRole participant);

    /// <summary>
    /// Represents the result of an event, including records and actions to be applied to participants.
    /// </summary>
    public class EventResult
    {
        /// <summary>
        /// Gets the list of event records.
        /// </summary>
        public List<EventRecord> Records { get; }

        /// <summary>
        /// Gets the action to be applied to the originator participant.
        /// </summary>
        public EventAction OriginatorAction { get; }

        /// <summary>
        /// Gets the action to be applied to the recipient participant.
        /// </summary>
        public EventAction RecipientAction { get; }

        public EventResultStatus Status { get; set; }

        public EventResultMeta? Meta { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventResult"/> class with no records or actions.
        /// </summary>
        public EventResult() : this(new List<EventRecord>(), null, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventResult"/> class with a single record.
        /// </summary>
        /// <param name="record">The event record.</param>
        /// <param name="originatorAction">The action to be applied to the originator participant.</param>
        /// <param name="recipientAction">The action to be applied to the recipient participant.</param>
        public EventResult(EventRecord record, EventAction originatorAction = null, EventAction recipientAction = null)
            : this(new List<EventRecord> { record }, originatorAction, recipientAction) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventResult"/> class with a list of records.
        /// </summary>
        /// <param name="records">The list of event records.</param>
        /// <param name="originatorAction">The action to be applied to the originator participant.</param>
        /// <param name="recipientAction">The action to be applied to the recipient participant.</param>
        public EventResult(List<EventRecord> records, EventAction originatorAction = null, EventAction recipientAction = null)
        {
            Records = records ?? throw new ArgumentNullException(nameof(records));
            OriginatorAction = originatorAction;
            RecipientAction = recipientAction;
        }

        /// <summary>
        /// Applies the defined actions to the participants.
        /// </summary>
        /// <param name="originator">The originator participant.</param>
        /// <param name="recipient">The recipient participant (optional).</param>
        public void ApplyActions(ParticipantRole originator, ParticipantRole recipient = null)
        {
            OriginatorAction?.Invoke(originator);
            RecipientAction?.Invoke(recipient);
        }

        /// <summary>
        /// Checks if there are any records with a non-null and non-empty message.
        /// </summary>
        /// <returns>True if there are any records with a non-null and non-empty message; otherwise, false.</returns>
        public bool HasRecords()
        {
            return Records.Any(record => !string.IsNullOrEmpty(record.Message));
        }

        public EventRecord? LastRecord()
        {
            return Records.LastOrDefault();
        }
    }
}
