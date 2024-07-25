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
        public List<EventResult> Messages { get; private set; }
        public int SelectedPackhorses { get; set; }
        public TimedActivityMeta? CurrentActivityMeta { get;set; }

        public Action CompleteActivity { get; set; }

        public event Func<Task> StateChanged;

        public SimulationViewModel()
        {
            HunterInstance = new Hunter("Kanta-ke");
            TraderInstance = new Trader("Bethabara");
            ExporterInstance = new Exporter("Charleston");
            Messages = new List<EventResult>();
            SelectedPackhorses = 1;

            HunterInstance.OnNotification += HandleNotification;
            TraderInstance.OnNotification += HandleNotification;
            ExporterInstance.OnNotification += HandleNotification;
        }

        public async Task Hunt()
        {
            var result = HunterInstance.Hunt(SelectedPackhorses);
            Messages.Add(result);
            await StateChanged?.Invoke();
        }

        public async Task SellToTrader(int numberOfSkins)
        {
            var result = HunterInstance.SellToTrader(TraderInstance, numberOfSkins);
            Messages.Add(result);
            await StateChanged?.Invoke();
        }

        public async Task TransportToExporter(int numberOfSkins)
        {
            var result = TraderInstance.TransportToExporter(ExporterInstance, numberOfSkins);
            Messages.Add(result);
            await StateChanged?.Invoke();
        }

        public async Task Export(int numberOfSkins)
        {
            var result = ExporterInstance.Export(numberOfSkins);
            Messages.Add(result);
            await StateChanged?.Invoke();
        }


        private async void HandleNotification(object sender, EventResult e)
        {
            Messages.Add(e);
            await StateChanged?.Invoke();
        }
    }
}
