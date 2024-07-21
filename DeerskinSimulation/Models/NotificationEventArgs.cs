using System;

namespace DeerskinSimulation.Models
{
    public class NotificationEventArgs : EventArgs
    {
        public object? Sender { get; }
        public string Message { get; }
        public string Color { get; }

        public NotificationEventArgs(string message, string color)
        {
            Message = message;
            Color = color;
        }

        public NotificationEventArgs(object sender, string message, string color) : this (message, color)
        {
            Sender = sender;
        }
    }
}
