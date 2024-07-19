using System;

namespace DeerskinSimulation.Models
{
    public class NotificationEventArgs : EventArgs
    {
        public string Message { get; }
        public string Color { get; }

        public NotificationEventArgs(string message, string color)
        {
            Message = message;
            Color = color;
        }
    }
}
