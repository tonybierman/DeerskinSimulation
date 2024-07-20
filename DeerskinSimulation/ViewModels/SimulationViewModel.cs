namespace DeerskinSimulation.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DeerskinSimulation.Models;

    public class SimulationViewModel
    {
        public Hunter HunterInstance { get; private set; }
        public Trader TraderInstance { get; private set; }
        public Exporter ExporterInstance { get; private set; }
        public List<NotificationEventArgs> Messages { get; private set; }
        public int SelectedPackhorses { get; set; }

        public event Func<Task> StateChanged;

        public SimulationViewModel()
        {
            HunterInstance = new Hunter("Hunter1");
            TraderInstance = new Trader("Trader1");
            ExporterInstance = new Exporter("Exporter1");
            Messages = new List<NotificationEventArgs>();
            SelectedPackhorses = 1;

            HunterInstance.OnNotification += HandleNotification;
            TraderInstance.OnNotification += HandleNotification;
            ExporterInstance.OnNotification += HandleNotification;
        }

        public async Task Hunt()
        {
            var result = HunterInstance.Hunt(SelectedPackhorses);
            Messages.Add(new NotificationEventArgs(result, "black"));
            await StateChanged?.Invoke();
        }

        public async Task SellToTrader()
        {
            var result = HunterInstance.SellToTrader(TraderInstance);
            Messages.Add(new NotificationEventArgs(result, "black"));
            await StateChanged?.Invoke();
        }

        public async Task TransportToExporter()
        {
            var result = TraderInstance.TransportToExporter(ExporterInstance);
            Messages.Add(new NotificationEventArgs(result, "black"));
            await StateChanged?.Invoke();
        }

        public async Task Export()
        {
            var result = ExporterInstance.Export();
            Messages.Add(new NotificationEventArgs(result, "black"));
            await StateChanged?.Invoke();
        }

        private async void HandleNotification(object sender, NotificationEventArgs e)
        {
            Messages.Add(e);
            await StateChanged?.Invoke();
        }
    }
}
