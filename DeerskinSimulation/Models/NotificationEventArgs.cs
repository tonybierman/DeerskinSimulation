using Castle.Core.Smtp;
using System;

namespace DeerskinSimulation.Models
{
    public class NotificationEventArgs : EventArgs
    {
        public EventRecord[] Records { get; }
        public object? Sender { get; }

        public NotificationEventArgs(EventRecord[] recs, object? sender = null)
        {
            Records = recs;
            Sender = sender;
        }
    }
}
